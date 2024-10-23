using System;
using System.Collections.Generic;

namespace BusinessLayer.Entity;

public partial class Cart
{
    public int CartId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Quantity { get; set; }

    public double? Price { get; set; }

    public bool? Status { get; set; }

/*    public virtual OrderItem? OrderItem { get; set; }
*/
    public virtual Product? Product { get; set; }

    public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

    public virtual User? User { get; set; }
}
