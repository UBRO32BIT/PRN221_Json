using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class BookCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
