insert into
    swd.customers (id, name, country_code, is_active)
values
    (1, 'Morten', 'UK', false),
    (2, 'Darren', 'UK', true),
    (3, 'Tobias', 'SE', false),
    (4, 'Athea', 'LT', true),
    (5, 'Adello', 'SW', true),
    (6, 'Jonas', 'US', true);
insert into
    swd.projects(id, name, customer_id, is_active)
values
    (1, 'Bayce', 1, false),
    (2, 'EPlus File Storage Service', 2, false),
    (3, 'EPlus Bluk Upload', 2, true),
    (4, 'Trygga', 3, false),
    (5, 'Rethink', 4, true),
    (6, 'AdCtrl Direct', 5, true),
    (7, 'Lead2Connect', 6, true);