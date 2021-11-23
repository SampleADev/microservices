using System;

using LinqToDB.Mapping;

namespace RdErp.Financial
{
    [Table(Name = "transaction_attributes", Schema = "fin")]
    public class FinancialTransactionAttribute
    {
        [Column("id"), Identity, PrimaryKey]
        public int Id { get; set; }

        [Column("transaction_id")]
        public int TransactionId { get; set; }

        [Column("attribute"), NotNull]
        public string Attribute { get; set; }

        [Column("ref")]
        public int Ref { get; set; }
    }
}