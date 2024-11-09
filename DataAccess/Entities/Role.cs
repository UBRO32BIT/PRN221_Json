using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace BusinessObject.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string RoleName { get; set; } = null!;

    [JsonIgnore]
    public virtual ICollection<Member> Members { get; } = new List<Member>();
}
