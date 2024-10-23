using System;
using System.Collections.Generic;

namespace BusinessLayer.Entity;

public partial class OrderItem
{
    public int OrderItemId { get; set; }

    public int? CartId { get; set; }

    public int? OrderId { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Cart? Cart { get; set; } 
}
