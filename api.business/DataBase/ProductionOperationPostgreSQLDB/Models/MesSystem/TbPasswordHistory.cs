using System;
using System.Collections.Generic;

namespace BusinessSQLDB.Models.MesSystem;

public partial class TbPasswordHistory
{
    /// <summary>
    /// User ID (Main Key)
    /// </summary>
    public string Id { get; set; } = null!;

    public int HistoryId { get; set; }

    public DateTime HistoryDate { get; set; }

    public string PasswordHash { get; set; } = null!;

    public virtual TbUser IdNavigation { get; set; } = null!;
}
