using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public DateOnly OrderCreateDate { get; set; }

    public TimeOnly OrderCreateTime { get; set; }

    public long? OrderClientId { get; set; }

    public string OrderStatus { get; set; } = null!;

    public DateOnly? OrderDateFinish { get; set; }

    public int OrderRentalTime { get; set; }

    public virtual Client? OrderClient { get; set; }
}
