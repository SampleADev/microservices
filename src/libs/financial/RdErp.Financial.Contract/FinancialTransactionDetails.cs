namespace RdErp.Financial
{
    public class FinancialTransactionDetails : FinancialTransaction
    {
        public bool CanEdit { get; set; }

        public FinancialTransactionAttribute[] Attributes { get; set; }

        public FinancialTransaction CorrelatedTransaction { get; set; }

        public FinancialTransaction[] TransactionsCorrelatedToThisTransaction { get; set; }
    }
}