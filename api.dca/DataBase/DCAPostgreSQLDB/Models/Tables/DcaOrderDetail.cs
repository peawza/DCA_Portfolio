using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class DcaOrderDetail
{
    public Guid DcaOrderDetailId { get; set; }

    public Guid DcaOrderId { get; set; }

    public int RowNo { get; set; }

    public string OrderSide { get; set; } = null!;

    public string AssetType { get; set; } = null!;

    public string Symbol { get; set; } = null!;

    public string? Market { get; set; }

    public decimal? PlannedPrice { get; set; }

    public decimal? PlannedAmount { get; set; }

    public bool ExecutedFlag { get; set; }

    public decimal? ExecutedPrice { get; set; }

    public decimal? ExecutedAmount { get; set; }

    public DateTime? ExecutedAt { get; set; }

    public string Status { get; set; } = null!;

    public string? BrokerOrderId { get; set; }

    public string? Note { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? UnrealizedPnlValue { get; set; }

    public decimal? CurrentMarketValue { get; set; }

    public virtual DcaOrderHeader DcaOrder { get; set; } = null!;
}
