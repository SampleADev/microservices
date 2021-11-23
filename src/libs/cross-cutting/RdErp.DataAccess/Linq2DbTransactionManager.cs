using System;
using System.Threading.Tasks;

using LinqToDB.Data;

using RdErp;

namespace RdErp.DataAccess
{

    class Linq2DbTransactionManager : ITransactionManager
    {
        private readonly DataConnection db;

        public Linq2DbTransactionManager(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public async Task<TResult> InTransaction<TResult>(Func<Task<TResult>> transactionalAction)
        {
            if (transactionalAction == null)
            {
                throw new ArgumentNullException(nameof(transactionalAction));
            }

            if (db.Transaction != null)
            {
                return await transactionalAction();
            }
            else
            {
                try
                {
                    db.BeginTransaction();

                    var result = await transactionalAction();

                    db.CommitTransaction();

                    return result;
                }
                catch
                {
                    db.RollbackTransaction();
                    throw;
                }
            }
        }

        public async Task InTransaction(Func<Task> transactionalAction)
        {
            await this.InTransaction(async() =>
            {
                await transactionalAction();
                return false;
            });
        }
    }
}