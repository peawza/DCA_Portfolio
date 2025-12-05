using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class DcaOrderHeader
{
    public Guid DcaOrderId { get; set; }

    public string? PlanName { get; set; }

    public string? AccountName { get; set; }

    public DateOnly OrderDate { get; set; }

    public decimal TargetBudget { get; set; }

    public decimal? TotalPlannedValue { get; set; }

    public decimal? TotalExecutedValue { get; set; }

    public decimal? TotalFee { get; set; }

    public string Status { get; set; } = null!;

    public string? Note { get; set; }

    public string CreatedBy { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? TotalUnrealizedPnl { get; set; }

    public decimal? TotalCurrentValue { get; set; }

    public virtual ICollection<DcaOrderDetail> DcaOrderDetails { get; set; } = new List<DcaOrderDetail>();
}
