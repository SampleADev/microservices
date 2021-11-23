using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using FluentValidation;

using LinqToDB;

using Mapster;

namespace RdErp.Financial
{
    class FinancialTransactionService : IFinancialTransactionService
    {
        private readonly IValidator<FinancialTransaction> transactionValidator;
        private readonly IValidator<FinancialTransactionAttribute> transactionAttributeValidator;
        private readonly ITransactionManager transactionManager;
        private readonly IFinancialTransactionDataAccessor financialDataAccessor;
        private readonly AppUser currentUser;
        private readonly IClock clock;

        public FinancialTransactionService(
            AppUser currentUser,
            IClock clock,
            IValidator<FinancialTransaction> transactionValidator,
            IValidator<FinancialTransactionAttribute> transactionAttributeValidator,
            ITransactionManager transactionManager,
            IFinancialTransactionDataAccessor financialDataAccessor
        )
        {
            this.currentUser = currentUser;
            this.clock = clock
                ??
                throw new ArgumentNullException(nameof(clock));
            this.transactionValidator = transactionValidator
                ??
                throw new ArgumentNullException(nameof(transactionValidator));
            this.transactionAttributeValidator = transactionAttributeValidator
                ??
                throw new ArgumentNullException(nameof(transactionAttributeValidator));
            this.transactionManager = transactionManager
                ??
                throw new ArgumentNullException(nameof(transactionManager));
            this.financialDataAccessor = financialDataAccessor
                ??
                throw new ArgumentNullException(nameof(financialDataAccessor));
        }

        public async Task<PageResult<FinancialTransactionInfo>> All(ListRequest request)
        {
            var query = (IQueryable<FinancialTransactionInfo>) financialDataAccessor.AllTransactionsInCurrency("UAH");

            if (!String.IsNullOrWhiteSpace(request.Search))
            {
                var isNumber = Decimal.TryParse(request.Search, out decimal searchNumber);

                query = query.Where(t =>
                    t.Description.ToLower().Contains(request.Search.ToLower())
                    || t.CostCenterName.Contains(request.Search)
                    || t.TransactionCodeName.Contains(request.Search)
                    || t.Currency == request.Search.ToUpper()
                    || (isNumber && t.Id == (int) searchNumber)
                    || (isNumber && (int) t.Amount == (int) searchNumber)
                );
            }

            request.SortBy = String.IsNullOrWhiteSpace(request.SortBy)
                ? nameof(FinancialTransaction.OccurredAt)
                : request.SortBy;

            return await query.ApplyListRequest(request,
                nameof(FinancialTransaction.Amount),
                nameof(FinancialTransactionInfo.TransactionCodeName),
                nameof(FinancialTransactionInfo.CostCenterName),
                nameof(FinancialTransactionInfo.Currency),
                nameof(FinancialTransactionInfo.OccurredAt),
                nameof(FinancialTransactionInfo.Id)
            );
        }

        public Task<FinancialTransactionInfo[]> AtMonth(DateTime month, string currency)
        {
            var startOfTheMonth = new DateTimeOffset(month.Year, month.Month, 1, 0, 0, 0, 0, TimeSpan.Zero);
            var endOfTheMonth = startOfTheMonth.AddMonths(1);

            return financialDataAccessor.AllTransactionsInCurrency(currency)
                .Where(t => t.OccurredAt >= startOfTheMonth && t.OccurredAt < endOfTheMonth)
                .ToArrayAsync();
        }

        public async Task<FinancialTransactionDetails> Get(int transactionId)
        {
            var transaction = await financialDataAccessor.GetTransaction(transactionId);
            if (transaction == null)
            {
                return null;
            }

            var attributes = await financialDataAccessor.GetTransactionAttributes(transactionId);
            var result = transaction.Adapt<FinancialTransactionDetails>();
            result.Attributes = attributes;
            result.TransactionsCorrelatedToThisTransaction = await financialDataAccessor
                .GetCorrelatedTransactions(transactionId)
                .ToArrayAsync();
            result.CorrelatedTransaction = result.CorrelatedTransactionId == null
                ? null
                : await financialDataAccessor.GetTransaction(result.CorrelatedTransactionId.Value);

            result.CanEdit = CanEditTransaction(result);

            return result;
        }

        public async Task<int> Register(FinancialTransaction transaction, IEnumerable<FinancialTransactionAttribute> attributes)
        {
            if (transaction == null) throw new ArgumentNullException(nameof(transaction));
            attributes = attributes ?? new FinancialTransactionAttribute[] { };

            if (transaction.Id == 0)
            {
                transaction.CreatedAt = DateTimeOffset.Now;
                transaction.CreatedBy = currentUser?.Id ?? "";

                transaction.CorrectionOfTransactionId = null;
            }
            else
            {
                var existingTransaction = await Get(transaction.Id);
                if (existingTransaction != null)
                {
                    transaction.Id = existingTransaction.Id;
                    transaction.CreatedAt = existingTransaction.CreatedAt;
                    transaction.CreatedBy = existingTransaction.CreatedBy;
                    transaction.CorrectionOfTransactionId = existingTransaction.CorrectionOfTransactionId;
                }
                else
                    throw new ArgumentException("Transaction is not found and can't be updated.");

                if (!CanEditTransaction(existingTransaction))
                {
                    throw new ArgumentException("Transaction can't be edited.");
                }
            }

            await transactionValidator
                .ValidateExtAsync(transaction, nameof(transaction))
                .AndThrow();
            await transactionAttributeValidator
                .ValidateExtAsync(attributes, nameof(attributes))
                .AndThrow();

            return await transactionManager.InTransaction(async() =>
            {
                if (transaction.Id == 0)
                {
                    var id = await financialDataAccessor.InsertTransaction(transaction);
                    await financialDataAccessor.InsertTransactionAttributes(id, attributes);

                    return id;
                }
                else
                {

                    await financialDataAccessor.UpdateTransaction(transaction);
                    await financialDataAccessor.DeleteTransactionAttributes(transaction.Id);
                    await financialDataAccessor.InsertTransactionAttributes(transaction.Id, attributes);

                    return transaction.Id;
                }

            });
        }

        private bool CanEditTransaction(FinancialTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException(nameof(transaction));
            }

            return Math.Abs((clock.Now() - transaction.CreatedAt).TotalDays) < 1.0;
        }
    }
}