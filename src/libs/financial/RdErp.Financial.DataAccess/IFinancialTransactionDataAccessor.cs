using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;

namespace RdErp.Financial
{
    public interface IFinancialTransactionDataAccessor
    {
        ITable<FinancialTransactionInfo> AllTransactionsInCurrency(string currency);

        Task<FinancialTransaction> GetTransaction(int transactionId);

        IQueryable<FinancialTransaction> GetCorrelatedTransactions(int transactionId);

        Task<FinancialTransactionAttribute[]> GetTransactionAttributes(int transactionId);

        Task<int> InsertTransaction(FinancialTransaction transaction);

        Task UpdateTransaction(FinancialTransaction transaction);

        Task InsertTransactionAttributes(int transactionId, IEnumerable<FinancialTransactionAttribute> attributes);

        Task DeleteTransactionAttributes(int transactionId);
    }
}