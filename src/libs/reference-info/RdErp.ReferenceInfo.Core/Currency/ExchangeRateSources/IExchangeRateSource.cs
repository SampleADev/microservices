using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RdErp.ReferenceInfo.Currency
{
    public interface IExchangeRateSource
    {
        string Name { get; }

        bool IsDefault { get; }

        Task<IEnumerable<ExchangeRate>> LoadExchangeRates(DateTime fromDate, DateTime? toDate);
    }
}