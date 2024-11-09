using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class Book
{
    public int BookId { get; set; }

    public int CategoryId { get; set; }

    public string BookName { get; set; } = null!;

    public string Author { get; set; } = null!;

    public string Publisher { get; set; } = null!;

    public int Year { get; set; }

    public string Description { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public int UnitsInStock { get; set; }

    public string ImageUrl { get; set; } = null!;
    [ValidateNever]
    [JsonIgnore]
    public virtual BookCategory Category { get; set; } = null!;
    [JsonIgnore]
    public virtual ICollection<OrderDetail> OrderDetails { get; } = new List<OrderDetail>();
}
