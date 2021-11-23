using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using LinqToDB;
using LinqToDB.Data;

namespace RdErp.ReferenceInfo.Currency
{
    public class Linq2DbExchangeRateDataAccessor : IExchangeRateDataAccessor
    {
        private readonly DataConnection db;

        public Linq2DbExchangeRateDataAccessor(DataConnection db)
        {
            this.db = db
                ??
                throw new ArgumentNullException(nameof(db));
        }

        public void Insert(IEnumerable<ExchangeRate> exchangeRates)
        {
            if (exchangeRates == null) throw new ArgumentNullException(nameof(exchangeRates));

            db.BulkCopy(new BulkCopyOptions()
            {
                BulkCopyType = BulkCopyType.MultipleRows,
                    NotifyAfter = 0
            }, exchangeRates);
        }
    }
}