using System;
using System.Collections.Generic;

namespace RdErp.ReferenceInfo.Currency
{
    public interface IExchangeRateDataAccessor
    {
        void Insert(IEnumerable<ExchangeRate> exchangeRates);
    }
}