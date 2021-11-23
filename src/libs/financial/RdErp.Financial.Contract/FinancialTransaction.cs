using System;

using LinqToDB.Mapping;

namespace RdErp.Financial
{
    /// <summary>
    /// Money movement.
    /// Registers and arrival or coming out an amount of money to/from some account.
    /// Account is abstract here means entity where money can enter from, exit to or be stored at.
    ///</summary>
    [Table(Name = "transactions", Schema = "fin")]
    public class FinancialTransaction
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        [Column("occurred_at")]
        public DateTimeOffset OccurredAt { get; set; }

        [Column("amount")]
        public decimal Amount { get; set; }

        [Column("currency")]
        public string Currency { get; set; }

        /// <summary>
        /// Gets or sets a money store to move money to/from it.
        /// Could be bank account or some virtual store like "cash".
        /// </summary>
        [Column("cost_center_id")]
        public int CostCenterId { get; set; }

        /// <summary>
        /// Gets or sets transaction code identifies a transaction kind.
        /// NOTE: Transaction codes should be static and defined before system usage.
        /// </summary>
        [Column("code")]
        public int Code { get; set; }

        [Column("description"), Nullable]
        public string Description { get; set; }

        [Column("external_transaction_id"), Nullable]
        public string ExternalTransactionId { get; set; }

        /// <summary>
        /// Gets or sets optional identifier of the transaction which were a base for generating
        /// this transaction.
        /// Some events could generate two or more transactions, and this property
        /// should store identifier of primary transaction.
        /// </summary>
        /// <returns></returns>
        [Column("correlated_transaction_id")]
        public int? CorrelatedTransactionId { get; set; }

        /// <summary>
        /// Gets or sets a transaction ID which is corrected by the current transaction.
        /// </summary>
        /// <returns></returns>
        [Column("correction_of_transaction_id"), Nullable]
        public int? CorrectionOfTransactionId { get; set; }

        /// <summary>
        /// A type of entity generates transaction.
        /// </summary>
        [Column("reference_type"), Nullable]
        public string ReferenceType { get; set; }

        /// <summary>
        /// An identifier of the entity generated transaction.
        /// </summary>
        [Column("reference_id"), Nullable]
        public int? ReferenceId { get; set; }

        [Column("created_at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Column("created_by"), Nullable]
        public string CreatedBy { get; set; }
    }
}