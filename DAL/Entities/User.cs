using System;
using System.Collections.Generic;

namespace DAL.EFCore.PostgreSQL;

public partial class User
{
    public long Id { get; set; }

    public string Email { get; set; } = null!;

    public bool EmailConfirmed { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = null!;

    public long ContactDataId { get; set; }

    public virtual ContactData ContactData { get; set; } = null!;
}
