using System;

using LinqToDB.Mapping;

namespace RdErp.Planning
{
    [Table(Name = "planning_events", Schema = "plan")]
    public class PlanningEvent
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        [Column("ordinal")]
        public int Ordinal { get; set; }

        [Column("occurred_at")]
        public DateTime OccurredAt { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

        [Column("cost_center_id"), NotNull]
        public int CostCenterId { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("is_changed_manually")]
        public bool IsChangedManually { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        // public int CreatedBy {get;set;}
        // Here is a little sense of storing created by since events always generated by system.
    }
}