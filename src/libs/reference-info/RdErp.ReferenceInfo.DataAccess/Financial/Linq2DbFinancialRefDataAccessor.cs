using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

namespace RdErp.ReferenceInfo.Financial
{
    public class Linq2DbFinancialRefDataAccessor : IFinancialRefDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbFinancialRefDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public IQueryable<CostCenterRef> GetCostCenterRef()
        {
            return db.GetTable<CostCenterRef>();
        }

        public IQueryable<TransactionCodeRef> GetTransactionCodes()
        {
            return db.GetTable<TransactionCodeRef>();
        }
    }
}