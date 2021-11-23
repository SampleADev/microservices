using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;
using static LinqToDB.Sql;

namespace RdErp.Financial
{
    class Linq2DbFinancialTransactionDataAccessor : IFinancialTransactionDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbFinancialTransactionDataAccessor(DataConnection db) => this.db = db
            ??
            throw new ArgumentNullException(nameof(db));

        [TableFunction("transactions_in_currency", Schema = "fin")]
        public ITable<FinancialTransactionInfo> AllTransactionsInCurrency(string currency)
        {
            return db.GetTable<FinancialTransactionInfo>(this, (MethodInfo) MethodInfo.GetCurrentMethod(), currency);
        }

        public IQueryable<FinancialTransactionInfo> FindAllTransactions() => db.GetTable<FinancialTransactionInfo>();

        public Task<FinancialTransaction> GetTransaction(int transactionId) => db.GetTable<FinancialTransaction>()
            .FirstOrDefaultAsync(t => t.Id == transactionId);

        public Task<FinancialTransactionAttribute[]> GetTransactionAttributes(int transactionId) =>
            db.GetTable<FinancialTransactionAttribute>()
            .Where(a => a.TransactionId == transactionId)
            .ToArrayAsync();

        public async Task<int> InsertTransaction(FinancialTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            transaction.Id = await db.InsertWithInt32IdentityAsync(transaction);
            return transaction.Id;
        }

        public async Task InsertTransactionAttributes(int transactionId, IEnumerable<FinancialTransactionAttribute> attributes)
        {
            if (attributes == null) throw new ArgumentNullException(nameof(attributes));

            foreach (var a in attributes)
            {
                a.TransactionId = transactionId;
                a.Id = await db.InsertWithInt32IdentityAsync(a);
            }
        }

        public async Task DeleteTransactionAttributes(int transactionId)
        {
            if (transactionId == 0) throw new ArgumentNullException(nameof(transactionId));
            await db.GetTable<FinancialTransactionAttribute>()
                .Where(a => a.TransactionId == transactionId)
                .DeleteAsync();
        }

        public async Task UpdateTransaction(FinancialTransaction transaction)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));

            await db.UpdateAsync(transaction);
        }

        public IQueryable<FinancialTransaction> GetCorrelatedTransactions(int transactionId) =>
            db.GetTable<FinancialTransaction>()
            .Where(t => t.CorrelatedTransactionId == transactionId);
    }
}