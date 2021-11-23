using System;
using System.Threading.Tasks;

namespace RdErp.ReferenceInfo.Currency
{
    public interface ICurrencyListService
    {
        Task<CurrencyRef[]> GetCurrencyCodes();
    }
}