using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Worker
{
    public int WorkerId { get; set; }

    public int? WorkerPost { get; set; }

    public string WorkerFirstName { get; set; } = null!;

    public string WorkerMidName { get; set; } = null!;

    public string? WorkerLastName { get; set; }

    public string WorkerLogin { get; set; } = null!;

    public string WorkerPassword { get; set; } = null!;

    public virtual ICollection<WorkerEnterDate> WorkerEnterDates { get; set; } = new List<WorkerEnterDate>();

    public virtual Post? WorkerPostNavigation { get; set; }
}
