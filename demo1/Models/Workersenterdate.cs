using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Workersenterdate
{
    public int Id { get; set; }

    public int? Workerid { get; set; }

    public DateTime? Workerlastenter { get; set; }

    public string? Workertypeenter { get; set; }
}
