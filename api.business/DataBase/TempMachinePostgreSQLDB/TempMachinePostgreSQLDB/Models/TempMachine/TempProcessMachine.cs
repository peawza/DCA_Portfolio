using System;
using System.Collections.Generic;

namespace TempMachinePostgreSQLDB.Models.TempMachine;

public partial class TempProcessMachine
{
    public string MachineCode { get; set; } = null!;

    public string ShiftId { get; set; } = null!;

    public DateTime? MachineStartTime { get; set; }

    public DateTime? MachineEndTime { get; set; }

    public int? MachineShiftStatus { get; set; }

    public int? MachineStatus { get; set; }

    public int? WipStatus { get; set; }

    public int? WorkOrderId { get; set; }

    public string? WorkOrderNo { get; set; }

    public int? WipLabelId { get; set; }

    public string? WipLabelNo { get; set; }

    public decimal? StandardCycleTime { get; set; }

    public int? MachinePlanQty { get; set; }

    public int? MachineActualQty { get; set; }

    public int? MachineOkQty { get; set; }

    public int? MachineNgQty { get; set; }

    public int? WipPlanQty { get; set; }

    public int? WipActualQty { get; set; }

    public int? WipOkQty { get; set; }

    public int? WipNgQty { get; set; }

    public string? EmployeeName { get; set; }

    public string? ServerProcessName { get; set; }

    public DateTime? StampTime { get; set; }
}
