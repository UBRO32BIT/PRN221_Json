using System;
using System.Collections.Generic;

namespace BusinessObject.Entities;

public partial class BookCategory
{
    public int CategoryId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; } = new List<Book>();
}
