using System;
using System.Threading.Tasks;

namespace RdErp.ReferenceInfo.Currency
{
    public class CurrencyListService : ICurrencyListService
    {
        public Task<CurrencyRef[]> GetCurrencyCodes()
        {
            return Task.FromResult(new CurrencyRef[]
            {

                new CurrencyRef { Code = "UAH", Currency = "UAH" },
                new CurrencyRef { Code = "USD", Currency = "$" },
                new CurrencyRef { Code = "EUR", Currency = "€" }
            });
        }
    }
}