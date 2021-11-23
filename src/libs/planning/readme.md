## Use cases

- Planning monthly income from outstaff-like project
  - Could be changed by adding/removing staff
- Planning income from fixed-price projects
  - Project may have several milestones
- Planning income from T&M projects
  - Income should be calculated based on working days
- Planning employee salary
  - It should be useful to exclude certain period (non-paid vacation)
- Planning monthly expenses
  - Paid each month at the day of month (like a rent)
  - Planning approximately expences like electricity, based on formula
- Planning weekly expenses like cleaning, performed on certain days of week
- Planning one-time expenses like HR fee
- Planning expenses split on few payments with dates

## Desired features

- Expenses/incoming should be grouped and
  each should be able to have few incomes and expenses under it.
  This would allow to view/edit related plans at once.
- Should be possible to view income/expenses by periods: by months, weeks, years
- Should be possible to compare plans (even past)
  with transactions (which are actual money movement registration).
- Events should be generated with amount in specific currency.
  Currency conversion should not be done by planning service.

## Domain

- Plan - a related set of future money income and outcome.
  Groups a plan schedules together.
- Plan Schedule - a schedule for several future income/outcome.
  The base for generating events.
- Event - a single planned income/outcome with defined amount and date.
  Events are generated based on plan schedule.
  Amount and date could be edited manually with saving log.

## Discussion

How to store plans.
There are few possible ways:

- Store events in database
- Generate events in code
- Generate events in SQL

## Q&A

Q: Is it needed to use events in SQL?

A: Probably yes, then it should be either stored or generated in SQL

---

Q: How to generate events on editing plan?

A:

1.  Update plan
2.  Choose a date from which a plan should be updated
3.  Remove events are future relative to change date.
4.  Archive a version of the plan

Actually that should be a splitting a plan to two plans,
regenerating evens for new and archiving old plan.

---

Q: How far in future events should be generated? When to regenerate it?

A: I guess one year should be ok.
Regeneration should be done manually or by cron-based task.
Also API for regenerating events should be exposed.

---

Q: Should events for plans be generated in SQL?

A: I guess much easier would be to generate it in code.
The amount of events is relatively small (thousands)
and generation performed rarely, so it is completely ok
to generate it with code and then add to database.

---

Q: How to support manually editing of events?

A: That is required feature which would make some cases much easier.
I think events should maintain a change log
(TBD, for now it is not necessary).
Manually edited events should not be treated in special way, and could be re-generated on changing plan.
Event should have mark that it were manually edit.

---

Q: What is the subject of versioning: plan or plan schedule?

A: I think plan schedule.
Copying the whole plan with ten schedules on editing a single schedule is not reasonable.

---

Q: How plans should be edited?

A: Fist of all plan schedules must be optionally time-limited.
Usually it would be not limited on creation
but eventually every schedule would have end date.

That means that there would be no generated events after end date.

Actually, plan versioning is the most unclear thing.

There are few options:

- Editing schedules (with keeping log).
  Events should be re-generated with cleaning-up old events.
- Splitting schedules (end one schedule and start new schedule from the date of ending).
  Events for ended plan after end date must be deleted.
- Splitting plans.
  Similar to splitting schedules, but applied to plan.
  It processes all schedules in the plan.

---
