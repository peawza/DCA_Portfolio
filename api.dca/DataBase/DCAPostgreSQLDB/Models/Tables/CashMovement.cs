using System;
using System.Collections.Generic;

namespace DCAPostgreSQLDB.Models.Tables;

public partial class CashMovement
{
    public Guid CashMovementId { get; set; }

    public string AccountName { get; set; } = null!;

    public DateTime MovementDate { get; set; }

    public string MovementType { get; set; } = null!;

    public decimal Amount { get; set; }

    public Guid? ReferenceId { get; set; }

    public string? Note { get; set; }
}
