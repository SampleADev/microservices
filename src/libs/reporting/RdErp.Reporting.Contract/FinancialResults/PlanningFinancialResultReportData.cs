using System;

namespace RdErp.Reporting.FinancialResults
{
    public class PlanningFinancialResultsReportData
    {
        public DateTime? PlanFactSplitDate { get; set; }

        public PlanningEventRecord[] EventRecords { get; set; }
    }
}