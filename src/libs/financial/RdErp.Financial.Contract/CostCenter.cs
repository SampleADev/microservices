using System;

using LinqToDB.Mapping;

namespace RdErp.Financial
{
    [Table(Name = "cost_center", Schema = "fin")]
    public class CostCenter
    {

        [Column("id"), PrimaryKey, Identity]
        public int Id { get; set; }

        [Column("name")]
        public int Name { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; }

        [Column("currency")]
        public string Currency { get; set; }
    }
}