using System;
using System.Collections.Generic;

namespace demo1.Models;

public partial class Post
{
    public int PostId { get; set; }

    public string PostName { get; set; } = null!;

    public virtual ICollection<Worker> Workers { get; set; } = new List<Worker>();
}
