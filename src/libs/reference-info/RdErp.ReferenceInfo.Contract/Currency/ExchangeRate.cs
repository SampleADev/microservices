using System;

using LinqToDB.Mapping;

namespace RdErp.ReferenceInfo.Currency
{
    /// <summary>
    /// Represents a cost of the one item of the target currency
    /// if source currency.
    /// /// </summary>
    [Table(Name = "exchange_rate", Schema = "ref")]
    public class ExchangeRate
    {
        [PrimaryKey, Identity, Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert from.
        /// </summary>
        [Column("from_curr"), NotNull]
        public string FromCurrency { get; set; }

        /// <summary>
        /// Gets or sets a currency to convert to.
        /// </summary>
        [Column("to_curr"), NotNull]
        public string ToCurrency { get; set; }

        /// <summary>
        /// Gets or sets a number to multiply
        /// amount in source currency to get an amount in target currency.
        /// </summary>
        [Column("rate")]
        public decimal Rate { get; set; }

        /// <summary>
        /// Gets or sets a date from which exchange rate is applied.
        /// </summary>
        [Column("at_date")]
        public DateTime At { get; set; }

        /// <summary>
        /// Gets or sets a free-text string indicates a source of the exchange rate.
        /// </summary>
        /// <value></value>
        [Column("rate_src"), Nullable]
        public string RateSource { get; set; }
    }
}