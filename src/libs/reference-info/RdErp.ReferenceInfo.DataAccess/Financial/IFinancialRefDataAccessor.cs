using System;
using System.Collections.Generic;
using System.Linq;

namespace RdErp.ReferenceInfo.Financial
{
    public interface IFinancialRefDataAccessor
    {
        IQueryable<TransactionCodeRef> GetTransactionCodes();

        IQueryable<CostCenterRef> GetCostCenterRef();
    }
}