using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Service
{
    public int ServiceId { get; set; }

    public string ServiceName { get; set; } = null!;

    public string ServiceCode { get; set; } = null!;

    public int ServiceCostPerHour { get; set; }
    
    public override string ToString()
    {
        return $"{ServiceName}";
    }
}
