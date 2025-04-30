using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class OrderAndService
{
    public int? OrderId { get; set; }

    public int? ServiceId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Service? Service { get; set; }
}
