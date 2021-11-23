using System;

namespace RdErp
{
    public class ListRequest
    {
        public int? Skip { get; set; }

        public int? Take { get; set; }

        public string Search { get; set; }

        public string SortBy { get; set; }

        public SortDirection? SortDirection { get; set; }
    }

    public enum SortDirection
    {
        Asc,
        Desc
    }
}