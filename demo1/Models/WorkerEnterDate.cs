using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class WorkerEnterDate
{
    public int EnterId { get; set; }

    public int? WorkerId { get; set; }

    public DateTime WorkerLastEnter { get; set; }

    public string WorkerEnterType { get; set; } = null!;

    public virtual Worker? Worker { get; set; }
}
