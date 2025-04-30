using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Client
{
    public long ClientId { get; set; }

    public string ClientFirstName { get; set; } = null!;

    public string ClientMidName { get; set; } = null!;

    public string? ClientLastName { get; set; }

    public decimal ClentPassport { get; set; }

    public DateOnly? ClientBirthday { get; set; }

    public string? ClientAddress { get; set; }

    public string ClientEmail { get; set; } = null!;

    public string ClientPassword { get; set; } = null!;

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    
    public override string ToString()
    {
        return $"{ClientLastName} {ClientFirstName} {ClientMidName}";
    }
}
