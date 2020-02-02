using System;
using System.Collections.Generic;
using System.Text;

namespace Shoplify.Domain.Enums
{
    public enum OrderStatus
    {
        InSeller = 1,
        InCourier = 2,
        Delivering = 3,
        Delivered = 4,
        Canceled = 5
    }
}
