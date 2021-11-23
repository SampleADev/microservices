using System;
using System.Threading.Tasks;

namespace RdErp
{
    /// <summary>
    /// Runs specified part of code inside a transaction (usually database transaction).
    /// Transaction would be commited at the end of the method execution or rolled back
    /// if method throws an exception.
    /// </summary>
    public interface ITransactionManager
    {
        Task<TResult> InTransaction<TResult>(Func<Task<TResult>> transactionalAction);

        Task InTransaction(Func<Task> transactionalAction);
    }
}