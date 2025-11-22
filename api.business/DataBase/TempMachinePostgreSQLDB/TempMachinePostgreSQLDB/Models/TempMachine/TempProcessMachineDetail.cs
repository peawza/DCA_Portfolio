using System;
using System.Collections.Generic;

namespace TempMachinePostgreSQLDB.Models.TempMachine;

public partial class TempProcessMachineDetail
{
    public string MachineCode { get; set; } = null!;

    public int IndexTime { get; set; }

    public TimeOnly TimeSeries { get; set; }

    public string? ShiftId { get; set; }

    public int? WorkOrderId { get; set; }

    public string? WorkOrderNo { get; set; }

    public int? WipLabelId { get; set; }

    public string? WipLabelNo { get; set; }

    public decimal? StandardCycleTime { get; set; }

    public int? MachineStatus { get; set; }

    public int? MachinePlanQty { get; set; }

    public int? MachineActualQty { get; set; }

    public int? MachineOkQty { get; set; }

    public int? MachineNgQty { get; set; }

    public decimal? MachineEfficiencyTarget { get; set; }

    public decimal? MachineOeeAvailability { get; set; }

    public decimal? MachineOeePerformance { get; set; }

    public decimal? MachineOeeQuality { get; set; }

    public decimal? MachineOeeTotal { get; set; }

    public int? WipStatus { get; set; }

    public int? WipPlanQty { get; set; }

    public int? WipActualQty { get; set; }

    public int? WipOkQty { get; set; }

    public int? WipNgQty { get; set; }

    public decimal? WipEfficiencyTarget { get; set; }

    public decimal? WipOeeAvailability { get; set; }

    public decimal? WipOeePerformance { get; set; }

    public decimal? WipOeeQuality { get; set; }

    public decimal? WipOeeTotal { get; set; }

    public string? EmployeeName { get; set; }

    public string? ServerProcessName { get; set; }

    public DateTime? StampTime { get; set; }
}
