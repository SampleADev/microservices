using System;

namespace RdErp
{
    public interface IClock
    {
        DateTimeOffset Now();

        DateTimeOffset Today();
    }
}