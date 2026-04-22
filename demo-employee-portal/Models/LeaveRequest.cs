namespace EmployeePortal.Models;

public class LeaveRequest
{
    public int Id { get; set; }
    public int EmployeeId { get; set; }
    public Employee? Employee { get; set; }

    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; } = "";
    public string Type { get; set; } = "VACATION";
    public string Status { get; set; } = "PENDING";

    public int ViewCount { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ApprovedAt { get; set; }
    public int? ApprovedBy { get; set; }
}
