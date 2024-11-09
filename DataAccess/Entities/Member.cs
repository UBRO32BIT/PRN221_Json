using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class Member
{
    public int MemberId { get; set; }

    public string Email { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string City { get; set; } = null!;

    public string Country { get; set; } = null!;

    public string Password { get; set; } = null!;

    public int RoleId { get; set; }

    [JsonIgnore]
    public virtual ICollection<Order> Orders { get; } = new List<Order>();

    [JsonIgnore]
    public virtual Role Role { get; set; } = null!;
}
