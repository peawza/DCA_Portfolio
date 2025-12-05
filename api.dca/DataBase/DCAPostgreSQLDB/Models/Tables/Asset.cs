using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class Asset
{
    public Guid AssetId { get; set; }

    public string Symbol { get; set; } = null!;

    public string Market { get; set; } = null!;

    public string AssetType { get; set; } = null!;

    public string? Name { get; set; }

    public string? Currency { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<AssetPriceHistory> AssetPriceHistories { get; set; } = new List<AssetPriceHistory>();

    public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
