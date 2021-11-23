using System;
using System.Threading.Tasks;

namespace RdErp.ReferenceInfo.Financial
{
    public interface IFinancialRefService
    {
        Task<TransactionCodeRef[]> GetTransactionCodes();

        Task<CostCenterRef[]> GetCostCenters();
    }
}