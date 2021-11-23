create extension if not exists tablefunc;

create or replace view fin.transaction_common_attributes
as
	select * from crosstab(
    	'select transaction_id, attribute, ref from fin.transaction_attributes order by 1',
    	$$VALUES
            ('CUSTOMER'),
            ('PROJECT '),
            ('EMPLOYEE')
        $$
    ) as ct(
            transaction_id int,
            "customer_id" int,
            "project_id" int,
            "employee_id" int);