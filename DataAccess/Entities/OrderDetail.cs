using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int BookId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public double? Discount { get; set; }

    [JsonIgnore]
    public virtual Book Book { get; set; } = null!;

    [JsonIgnore]
    public virtual Order Order { get; set; } = null!;
}
