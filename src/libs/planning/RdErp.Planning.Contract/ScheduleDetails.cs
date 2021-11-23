using System;

using LinqToDB.Mapping;

namespace RdErp.Planning
{
    public class ScheduleDetails : Schedule
    {
        public ScheduleAttribute[] Attributes { get; set; }
    }
}