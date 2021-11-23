using System;

using LinqToDB.Mapping;

namespace RdErp.Planning
{
    [Table(Name = "plan", Schema = "plan")]
    public class Plan
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        [Column("name"), NotNull]
        public string Name { get; set; }

        [Column("active")]
        public bool IsActive { get; set; }
    }
}