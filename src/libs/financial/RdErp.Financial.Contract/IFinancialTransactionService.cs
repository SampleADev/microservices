using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RdErp.Financial
{
    public interface IFinancialTransactionService
    {
        /// <summary>
        /// Registers new transaction and returns identifier of the new transaction.
        /// </summary>
        Task<int> Register(FinancialTransaction transaction, IEnumerable<FinancialTransactionAttribute> attributes);

        /// <summary>
        /// Finds all transactions
        /// </summary>
        Task<PageResult<FinancialTransactionInfo>> All(ListRequest request);

        /// <summary>
        /// Gets all transactions at specified month.
        /// </summary>
        Task<FinancialTransactionInfo[]> AtMonth(DateTime month, string currency);

        /// <summary>
        /// Get transaction with attributes by identifier.
        /// </summary>
        Task<FinancialTransactionDetails> Get(int transactionId);
    }
}