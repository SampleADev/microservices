using LinqToDB.Mapping;

namespace RdErp.Planning
{
    public class PlanningEventInfo : PlanningEvent
    {
        [Column("plan_id")]
        public int PlanId { get; set; }

        [Column("plan_name")]
        public string PlanName { get; set; }

        [Column("cost_center_name")]
        public string CostCenterName { get; set; }

        [Column("schedule_name")]
        public string ScheduleName { get; set; }

        [Column("converted_to_currency")]
        public string ConvertedToCurrency { get; set; }

        [Column("converted_amount")]
        public decimal ConvertedAmount { get; set; }
    }
}