create
or replace function fin.transactions_in_currency(currency varchar(3)) returns table (
    id int,
    occurred_at timestamp with time zone,
    amount numeric(18, 2),
    currency varchar(3),
    converted_to_currency varchar(3),
    converted_amount numeric(18, 2),
    cost_center_id int,
    cost_center_name varchar(255),
    code int,
    transaction_code_name varchar(255),
    correlated_transaction_id int,
    correction_of_transaction_id int,
    description varchar(1024),
    external_transaction_id varchar(1024),
    reference_type char(10),
    reference_id int,
    created_at timestamp with time zone,
    created_by varchar(255)
) as $$
select
    t.id,
    t.occurred_at,
    t.amount,
    t.currency,
    $1 converted_to_currency,
    coalesce(
        (
            case
                when t.currency = $1 then t.amount
                else t.amount * (
                    select
                        r.rate
                    from
                        ref.exchange_rate_expanded r
                    where
                        r.from_curr = t.currency
                        and r.to_curr = $1
                        and r.at_date <= t.occurred_at
                    order by
                        r.at_date desc
                    limit
                        1
                )
            end
        ), t.amount
    ) converted_amount, t.cost_center_id, t.cost_center_name, t.code, t.transaction_code_name, t.correlated_transaction_id, t.correction_of_transaction_id, t.description, t.external_transaction_id, t.reference_type, t.reference_id, t.created_at, t.created_by
from
    fin.transactions_info t $$ language sql;