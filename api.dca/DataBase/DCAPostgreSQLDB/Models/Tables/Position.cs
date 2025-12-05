using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class Position
{
    public Guid PositionId { get; set; }

    public string AccountName { get; set; } = null!;

    public Guid AssetId { get; set; }

    public decimal TotalQty { get; set; }

    public decimal TotalCost { get; set; }

    public decimal AvgCost { get; set; }

    public decimal? CurrentPrice { get; set; }

    public decimal? CurrentValue { get; set; }

    public decimal? UnrealizedPnl { get; set; }

    public decimal RealizedPnl { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Asset Asset { get; set; } = null!;
}
