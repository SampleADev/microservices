using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

namespace RdErp.Planning
{
    class Linq2DbPlanDataAccessor : IPlanDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbPlanDataAccessor(DataConnection db) => this.db = db
            ??
            throw new ArgumentNullException(nameof(db));

        public IQueryable<Plan> All()
        {
            return db.GetTable<Plan>();
        }

        public Task<Plan> GetById(int id)
        {
            return db.GetTable<Plan>().FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<int> Insert(Plan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            plan.Id = await db.InsertWithInt32IdentityAsync(plan);

            return plan.Id;
        }

        public async Task Update(Plan plan)
        {
            if (plan == null)
            {
                throw new ArgumentNullException(nameof(plan));
            }

            await db.UpdateAsync(plan);
        }

        public async Task Delete(int planId)
        {
            await db.GetTable<Plan>()
                .Where(e => e.Id == planId)
                .DeleteAsync();
        }

        // public IQueryable<FinancialTransaction> FindAllTransactions() => db.GetTable<FinancialTransaction>();

        // public Task<FinancialTransaction> GetTransaction(int transactionId) => db.GetTable<FinancialTransaction>()
        //     .FirstOrDefaultAsync(t => t.Id == transactionId);

        // public Task<FinancialTransactionAttribute[]> GetTransactionAttributes(int transactionId) =>
        //     db.GetTable<FinancialTransactionAttribute>()
        //     .Where(a => a.TransactionId == transactionId)
        //     .ToArrayAsync();

        // public async Task<int> InsertTransaction(FinancialTransaction transaction)
        // {
        //     if (transaction == null) throw new ArgumentNullException(nameof(transaction));

        //     transaction.Id = await db.InsertWithInt32IdentityAsync(transaction);
        //     return transaction.Id;
        // }

        // public async Task InsertTransactionAttributes(int transactionId, IEnumerable<FinancialTransactionAttribute> attributes)
        // {
        //     if (attributes == null) throw new ArgumentNullException(nameof(attributes));

        //     foreach (var a in attributes)
        //     {
        //         a.TransactionId = transactionId;
        //         a.Id = await db.InsertWithInt32IdentityAsync(a);
        //     }
        // }
    }
}