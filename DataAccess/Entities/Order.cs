using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class Order
{
    public int OrderId { get; set; }

    public int MemberId { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? ShippedDate { get; set; }

    public decimal? TotalPrice { get; set; }

    public virtual Member Member { get; set; } = null!;

    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
