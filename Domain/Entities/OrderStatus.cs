
﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public enum OrderStatus
    {
        Processing = 0,
        Shipped=1,
        Delivered=2,
        Cancelled = 3
    }
}
