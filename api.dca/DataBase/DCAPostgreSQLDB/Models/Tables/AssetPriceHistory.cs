using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class AssetPriceHistory
{
    public Guid AssetId { get; set; }

    public DateOnly PriceDate { get; set; }

    public decimal ClosePrice { get; set; }

    public virtual Asset Asset { get; set; } = null!;
}
