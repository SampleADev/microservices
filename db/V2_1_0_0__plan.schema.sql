create schema if not exists "plan";

create table if not exists plan.plan (
    id int not null primary key generated by default as identity,
    name varchar(512) not null,
    active boolean not null default(true)
);

create table if not exists plan.schedule (
    id int not null primary key generated by default as identity,
    plan_id int not null
        references plan.plan(id),
    name varchar(512) not null,
    start_date date not null,
    end_date date null,
    active boolean not null default(true),
    currency varchar(3) not null,
    schedule_rule varchar(128) not null,
    schedule_settings text not null,
    value_expression text not null,
    cost_center_id int not null
        references fin.cost_center(id)
);

create table if not exists plan.schedule_attributes (
    id int not null primary key generated by default as identity,
    schedule_id int not null
        references plan.schedule(id),
    attribute varchar(20) not null,
    ref int not null
);

create table if not exists plan.planning_events (
    id int not null primary key generated by default as identity,
    ordinal int not null,
    occurred_at date not null,
    amount numeric(18, 2) not null,
    currency varchar(3) not null,
    cost_center_id int not null
        references fin.cost_center(id),
    schedule_id int not null
        references plan.schedule(id),
    is_changed_manually boolean not null default(false),
    created_at timestamp with time zone not null default (current_timestamp)
);