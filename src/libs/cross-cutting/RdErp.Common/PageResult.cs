using System;

namespace RdErp
{
    public class PageResult<T>
        {
            public T[] Data { get; set; }

            public int Total { get; set; }
        }
}