using System;
using System.Threading.Tasks;

using LinqToDB;

namespace RdErp.ReferenceInfo.Financial
{
    public class FinancialRefService : IFinancialRefService
    {
        private readonly IFinancialRefDataAccessor dataAccessor;

        public FinancialRefService(
            IFinancialRefDataAccessor dataAccessor
        )
        {
            this.dataAccessor = dataAccessor
                ??
                throw new ArgumentNullException(nameof(dataAccessor));
        }

        public Task<CostCenterRef[]> GetCostCenters() =>
            dataAccessor.GetCostCenterRef().ToArrayAsync();

        public Task<TransactionCodeRef[]> GetTransactionCodes() =>
            dataAccessor.GetTransactionCodes().ToArrayAsync();
    }
}