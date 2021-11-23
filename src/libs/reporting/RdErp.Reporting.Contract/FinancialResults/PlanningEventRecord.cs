using System;

using LinqToDB.Mapping;

namespace RdErp.Reporting.FinancialResults
{
    public class PlanningEventRecord
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("ordinal")]
        public int Ordinal { get; set; }

        [Column("occurred_at")]
        public DateTime OccurredAt { get; set; }

        [Column("occurred_at_month")]
        public DateTime OccurredAtMonth { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("schedule_name")]
        public string ScheduleName { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Positive or negative amount of this event represents.
        /// </summary>
        [Column("amount")]
        public decimal Amount { get; set; }

        /// <summary>
        /// Sliding sum of all amounts of prior events and this event.
        /// </summary>
        [Column("sliding_total")]
        public decimal SlidingTotal { get; set; }

        /// <summary>
        /// Sum of all events belongs to the same month.
        /// </summary>
        [Column("monthly_total")]
        public decimal MonthlyTotal { get; set; }

        /// <summary>
        /// Sum of amount of all events belongs to the same month and prior events.
        /// </summary>
        [Column("monthly_total_sliding")]
        public decimal MonthlyTotalSliding { get; set; }
    }
}