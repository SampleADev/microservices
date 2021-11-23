using System;

using LinqToDB.Mapping;

namespace RdErp.Reporting.FinancialResults
{
    public class MonthlyPlanFactResultsRecord
    {
        [Column("occurred_at_month")]
        public DateTime OccurredAtMonth { get; set; }

        [Column("plan_monthly_total")]
        public decimal? PlanMonthlyTotal { get; set; }

        [Column("plan_monthly_income")]
        public decimal? PlanMonthlyIncome { get; set; }

        [Column("plan_monthly_expence")]
        public decimal? PlanMonthlyExpence { get; set; }

        [Column("fact_monthly_total")]
        public decimal? FactMonthlyTotal { get; set; }

        [Column("fact_monthly_income")]
        public decimal? FactMonthlyIncome { get; set; }

        [Column("fact_monthly_expence")]
        public decimal? FactMonthlyExpence { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

    }
}