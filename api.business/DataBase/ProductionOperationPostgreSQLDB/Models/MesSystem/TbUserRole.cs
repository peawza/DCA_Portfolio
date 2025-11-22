using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbUserRole
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public string Discriminator { get; set; } = null!;

    public virtual TbRole Role { get; set; } = null!;

    public virtual TbUser User { get; set; } = null!;
}
