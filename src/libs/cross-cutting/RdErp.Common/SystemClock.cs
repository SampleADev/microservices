using System;

namespace RdErp
{
    public class SystemClock : IClock
    {
        public DateTimeOffset Now() => DateTimeOffset.Now;

        public DateTimeOffset Today() => Now().Date;
    }
}