using System;

using LinqToDB.Mapping;

namespace RdErp.ReferenceInfo.Financial
{
    [Table(Name = "cost_center", Schema = "fin")]
    public class CostCenterRef
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert from.
        /// </summary>
        [Column("name"), NotNull]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert to.
        /// </summary>
        [Column("is_active"), NotNull]
        public bool IsActive { get; set; }

        [Column("currency")]
        public string Currency { get; set; }
    }
}