using System;

using LinqToDB.Mapping;

namespace RdErp.ReferenceInfo.Financial
{
    [Table(Name = "transaction_codes", Schema = "fin")]
    public class TransactionCodeRef
    {
        [PrimaryKey, Identity, Column("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert from.
        /// </summary>
        [Column("name"), NotNull]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert to.
        /// </summary>
        [Column("description"), NotNull]
        public string Description { get; set; }

        [Column("is_active"), NotNull]
        public bool IsActive { get; set; }
    }
}