using System;

using LinqToDB.Mapping;

namespace RdErp.Planning
{
    [Table(Name = "schedule_attributes", Schema = "plan")]
    public class ScheduleAttribute
    {
        [Column("id"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Column("schedule_id")]
        public int ScheduleId { get; set; }

        [Column("attribute"), NotNull]
        public string Attribute { get; set; }

        [Column("ref")]
        public int Ref { get; set; }
    }
}