using System;

using LinqToDB.Mapping;

namespace RdErp.Financial
{
    [Table(Name = "transactions_info", Schema = "fin")]
    public class FinancialTransactionInfo : FinancialTransaction
    {
        [Column("cost_center_name")]
        public string CostCenterName { get; set; }

        /// <summary>
        /// Currency of ConvertedAmount
        /// </summary>
        /// <value></value>
        [Column("converted_to_currency")]
        public string ConvertedToCurrency { get; set; }

        /// <summary>
        ///Amount in currency one for all resultset
        /// </summary>
        /// <value></value>
        [Column("converted_amount")]
        public decimal ConvertedAmount { get; set; }

        [Column("transaction_code_name")]
        public string TransactionCodeName { get; set; }
    }
}