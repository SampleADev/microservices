using System;

using LinqToDB.Mapping;

namespace RdErp.Planning
{
    [Table(Name = "schedule", Schema = "plan")]
    public class Schedule
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        [Column("plan_id")]
        public int PlanId { get; set; }

        [Column("name"), NotNull]
        public string Name { get; set; }

        [Column("currency"), NotNull]
        public string Currency { get; set; }

        [Column("cost_center_id"), NotNull]
        public int CostCenterId {get;set;}

        /// <summary>
        /// Start date. Time part is ignored.
        /// </summary>
        /// <value></value>
        [Column("start_date")]
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Optional end date. Time part is ignored.
        /// </summary>
        /// <value></value>
        [Column("end_date"), Nullable]
        public DateTime? EndDate { get; set; }

        [Column("active")]
        public bool IsActive { get; set; }

        [Column("schedule_rule"), NotNull]
        public string ScheduleRule { get; set; }

        /// <summary>
        /// Gets or sets JSON-serialized schedule settings.
        /// Actual type depends on schedule rule.
        /// </summary>
        [Column("schedule_settings"), NotNull]
        public string ScheduleSettings { get; set; }

        [Column("value_expression"), NotNull]
        public string ValueExpression { get; set; }
    }
}