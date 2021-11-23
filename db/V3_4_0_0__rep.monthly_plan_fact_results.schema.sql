-- Gets the list of planned and fact incomes and expences by month
create
or replace function rep.monthly_plan_fact_results(currency varchar(3)) returns table (
    occurred_at_month date,
    plan_monthly_total numeric(18, 2),
    plan_monthly_income numeric(18, 2),
    plan_monthly_expence numeric(18, 2),
    fact_monthly_total numeric(18, 2),
    fact_monthly_income numeric(18, 2),
    fact_monthly_expence numeric(18, 2),
    currency varchar(3)
) as $$ with plans as (
    select
        date_trunc('month', e.occurred_at) as occurred_at_month,
        sum(
            case
                when e.converted_amount > 0 then e.converted_amount
                else 0
            end
        ) as monthly_income,
        sum(
            case
                when e.converted_amount < 0 then e.converted_amount
                else 0
            end
        ) as monthly_expence,
        sum(e.converted_amount) as monthly_total
    from
        plan.planning_events_info($1) e
    group by
        date_trunc('month', e.occurred_at)
),
trans as (
    select
        date_trunc('month', f.occurred_at) as occurred_at_month,
        sum(
            case
                when f.converted_amount > 0 then f.converted_amount
                else 0
            end
        ) as monthly_income,
        sum(
            case
                when f.converted_amount < 0 then f.converted_amount
                else 0
            end
        ) as monthly_expence,
        sum(f.converted_amount) as monthly_total
    from
        fin.transactions_in_currency($1) f
    group by
        date_trunc('month', f.occurred_at)
)
select
    coalesce(p.occurred_at_month, t.occurred_at_month) :: date occurred_at_month,
    p.monthly_total plan_monthly_total,
    p.monthly_income plan_monthly_income,
    p.monthly_expence plan_monthly_expence,
    t.monthly_total fact_monthly_total,
    t.monthly_income fact_monthly_income,
    t.monthly_expence fact_monthly_expence,
    $1 currency
from
    plans p full
    outer join trans t on p.occurred_at_month = t.occurred_at_month $$ LANGUAGE SQL;