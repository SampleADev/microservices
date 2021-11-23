create schema if not exists rep;

-- Gets the list of planned incomes and expenses converted to given currency.
-- Additionaly returns:
-- * sliding total sum on enterprise balance after each numeric(18, 2) movement.
-- * monthly isolated balance (sum of all expences and incomes for the given month)
-- * monthly sliding total sum
--
-- Parameters:
--    currency - a currency to convert all amounts to
--    actualization_date - only events after that date is aggregated.
--                         Before that date planning events are ignored, instead
--                         financial transaction is aggregated to start planning from
--                         actual amounts.
-- Notes:
-- For actualization transaction are aggregated strictly less before actualization date.
-- Events are aggregated including actualization date.
-- This assumes that fin transactions all entered till the end of the date,
--    and transactions for actualization date would be similar to planning amounts,
--    so not entered yet fin transactions would have less effect on report results.
create or replace function rep.planned_financial_results(currency varchar(3), actualization_date date)
    returns table (
        id int,
        ordinal int,
        occurred_at date,
        occurred_at_month date,
        schedule_id int,
        schedule_name varchar(512),
        amount numeric(18, 2),
        currency varchar(3),
        sliding_total numeric(18, 2),
        monthly_total numeric(18, 2),
        monthly_total_sliding numeric(18, 2)
    )
as
$$
with
actualization_event as (
    select
        -1 as id,
        -1 as ordinal,
        $2 as occurred_at,
        coalesce(sum(t.amount), 0::numeric(18, 2)) amount,
        $1 currency,
        -1 as schedule_id,
        '--actualization--' as schedule_name,
        '1970-01-01'::date as start_date,
        $2 as end_date
    from
        fin.transactions_in_currency($1) t
    where
        t.occurred_at::date < $2
),
-- Active events
event_info as
(
    select
        e.id,
        e.ordinal,
        e.occurred_at::date,
        e.amount,
        e.currency,
        s.id schedule_id,
        s.name schedule_name,
        s.start_date,
        s.end_date
    from
        plan.planning_events e inner join
        plan.schedule s on e.schedule_id = s.id inner join
        plan.plan p on s.plan_id = p.id
    where
        s.active = true and
        p.active = true and
        e.occurred_at >= $2

    union all

    select * from actualization_event
),
-- In given currency
event_curr as
(
    select
        e.id,
        e.ordinal,
        e.occurred_at,
        e.schedule_id,
        e.schedule_name,
        (
            case
                when e.currency = $1 then e.amount
                else
                    e.amount * (
                        select
                            r.rate
                        from
                            ref.exchange_rate_expanded r
                        where
                            r.from_curr = e.currency and
                            r.to_curr = $1 and
                            r.at_date <= e.occurred_at
                        order by
                            r.at_date desc
                        limit 1
                    )
            end
        ) amount,
        $1 currency
    from
        event_info e
),
-- Aggregated in sliding sum by event and
-- total sum per month
event_sum as
(
    select
        *,
        sum(e.amount) over(order by e.occurred_at, e.ordinal, e.id) as sliding_total
    from
        event_curr e
),
event_monthly as
(
    select
        date_trunc('month', e.occurred_at) occurred_at_month,
        sum(e.amount) monthly_total
    from
        event_curr e
    group by
        date_trunc('month', e.occurred_at)
),
event_monthly_agg as
(
    select
        e.*,
        sum(e.monthly_total)
			over(order by e.occurred_at_month) monthly_total_sliding
    from
        event_monthly e
)
select
    e.id,
    e.ordinal,
    e.occurred_at,
    m.occurred_at_month::date occurred_at_month,
    e.schedule_id,
    e.schedule_name,
    e.amount,
    e.currency,
	e.sliding_total,
    m.monthly_total,
    m.monthly_total_sliding
from
    event_sum e inner JOIN
    event_monthly_agg m on date_trunc('month', e.occurred_at) = m.occurred_at_month
order by
    e.occurred_at
$$
LANGUAGE SQL;