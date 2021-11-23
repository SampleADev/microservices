using System;

using LinqToDB.Mapping;

namespace RdErp.Financial
{
    public enum FinancialTransactionType
    {

        ///<summary>
        /// Transaction increases an amount of money available for the enterprise.
        /// Example: customer have paid an invoice.
        ///</summary>
        [MapValue("I")]
        Income,

        ///<summary>
        /// Transaction decreases an amount of money available for the enterprise.
        /// Example: salary is paid, tax is paid.
        ///</summary>
        [MapValue("O")]
        Outcome
    }
}