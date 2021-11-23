create
or replace function plan.planning_events_info(currency varchar(3)) returns table (
    id int,
    ordinal int,
    occurred_at date,
    amount numeric(18, 2),
    currency varchar(3),
    cost_center_id int,
    schedule_id int,
    is_changed_manually boolean,
    created_at timestamp with time zone,
    schedule_name varchar(255),
    plan_id int,
    cost_center_name varchar(255),
    plan_name varchar(255),
    converted_to_currency varchar(3),
    converted_amount numeric(18, 2)
) as $$
select
    e.id,
    e.ordinal,
    e.occurred_at,
    e.amount,
    e.currency,
    e.cost_center_id,
    e.schedule_id,
    e.is_changed_manually,
    e.created_at,
    s.name as schedule_name,
    s.plan_id,
    cc.name as cost_center_name,
    p.name as plan_name,
    $1 converted_to_currency,
    coalesce(
        (
            case
                when e.currency = $1 then e.amount
                else e.amount * (
                    select
                        r.rate
                    from
                        ref.exchange_rate_expanded r
                    where
                        r.from_curr = e.currency
                        and r.to_curr = $1
                        and r.at_date <= e.occurred_at
                    order by
                        r.at_date desc
                    limit
                        1
                )
            end
        ), e.amount
    ) converted_amount
from
    plan.planning_events as e
    inner join plan.schedule as s on e.schedule_id = s.id
    inner join plan.plan as p on s.plan_id = p.id
    inner join fin.cost_center as cc on s.cost_center_id = cc.id $$ language sql;