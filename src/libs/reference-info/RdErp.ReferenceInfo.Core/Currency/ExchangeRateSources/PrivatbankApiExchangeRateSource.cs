using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace RdErp.ReferenceInfo.Currency
{
    public class PrivatbankApiExchangeRateSource : IExchangeRateSource
    {
        public string Name => "PB API";

        public bool IsDefault => true;

        public async Task<IEnumerable<ExchangeRate>> LoadExchangeRates(DateTime fromDate, DateTime? toDate)
        {
            var date = DateTime.Now.Date;

            using(var client = new HttpClient())
            {
                var response = await client.GetStringAsync(
                    "https://api.privatbank.ua/p24api/pubinfo?json&exchange&coursid=5"
                );

                var result = JsonConvert.DeserializeObject<PbExchangeRate[]>(response);

                return result
                    .Where(r => StringComparer.OrdinalIgnoreCase.Equals(r.Currency, "USD"))
                    .SelectMany(r =>
                    {

                        return new ExchangeRate[]
                        {
                        new ExchangeRate
                        {
                        RateSource = Name,
                        At = date,
                        FromCurrency = r.Currency,
                        ToCurrency = r.BaseCurrency,
                        Rate = r.BuyRate
                        },
                        new ExchangeRate
                        {
                        RateSource = Name,
                        At = date,
                        FromCurrency = r.BaseCurrency,
                        ToCurrency = r.Currency,
                        Rate = 1 / r.SaleRate
                        }
                        };

                    })
                    .ToArray();

            }
        }

    }

    /// <summary>
    /// {
    ///   "ccy": "USD",
    ///   "base_ccy": "UAH",
    ///   "buy": "26.95000",
    ///   "sale": "27.15000"
    /// }
    /// </summary>
    class PbExchangeRate
    {
        [JsonProperty("ccy")]
        public string Currency { get; set; }

        [JsonProperty("base_ccy")]
        public string BaseCurrency { get; set; }

        [JsonProperty("sale")]
        public decimal SaleRate { get; set; }

        [JsonProperty("buy")]
        public decimal BuyRate { get; set; }
    }
}