using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RdErp.ReferenceInfo.Currency
{
    public interface IExchangeRateService
    {
        Task RefreshExchangeRates(string rateSource);
    }
}