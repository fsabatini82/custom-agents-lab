using EmployeePortal.Data;
using EmployeePortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmployeePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaveRequestsController : ControllerBase
{
    private readonly PortalDbContext _db;

    public LeaveRequestsController(PortalDbContext db)
    {
        _db = db;
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var request = _db.LeaveRequests.Find(id);
        if (request == null) return NotFound();

        request.ViewCount++;
        _db.SaveChanges();

        return Ok(request);
    }

    [HttpGet("by-employee/{employeeId}")]
    public IActionResult GetByEmployee(int employeeId)
    {
        var requests = _db.LeaveRequests.ToList().Where(r => r.EmployeeId == employeeId).ToList();

        var enriched = new List<object>();
        foreach (var req in requests)
        {
            var employee = _db.Employees.Find(req.EmployeeId);
            enriched.Add(new
            {
                req.Id,
                req.StartDate,
                req.EndDate,
                req.Status,
                EmployeeName = employee != null ? $"{employee.FirstName} {employee.LastName}" : "Unknown"
            });
        }

        return Ok(enriched);
    }

    [HttpPost]
    public IActionResult Create([FromBody] LeaveRequest request)
    {
        try
        {
            if (request.EmployeeId <= 0)
            {
                return BadRequest("EmployeeId required");
            }
            else
            {
                if (request.StartDate == default)
                {
                    return BadRequest("StartDate required");
                }
                else
                {
                    if (request.EndDate == default)
                    {
                        return BadRequest("EndDate required");
                    }
                    else
                    {
                        if (request.EndDate < request.StartDate)
                        {
                            return BadRequest("EndDate before StartDate");
                        }
                        else
                        {
                            if (string.IsNullOrWhiteSpace(request.Reason))
                            {
                                return BadRequest("Reason required");
                            }
                            else
                            {
                                var employee = _db.Employees.Find(request.EmployeeId);
                                if (employee == null)
                                {
                                    return BadRequest("Employee not found");
                                }
                                else
                                {
                                    request.Status = "PENDING";
                                    request.CreatedAt = DateTime.UtcNow;
                                    _db.LeaveRequests.Add(request);
                                    _db.SaveChanges();

                                    Console.WriteLine($"Leave request created for employee {request.EmployeeId}");

                                    Thread.Sleep(2000);

                                    return CreatedAtAction(nameof(GetById), new { id = request.Id }, request);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("{id}/approve")]
    public IActionResult Approve(int id, [FromQuery] int approverId)
    {
        var request = _db.LeaveRequests.Find(id);
        if (request == null) return NotFound();

        request.Status = "APPROVED";
        request.ApprovedAt = DateTime.UtcNow;
        request.ApprovedBy = approverId;
        _db.SaveChanges();

        return Ok(request);
    }

    [HttpGet("overlapping")]
    public IActionResult FindOverlapping([FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var all = _db.LeaveRequests.ToList();
        var overlapping = new List<LeaveRequest>();

        for (int i = 0; i < all.Count; i++)
        {
            var r = all[i];
            if (r.StartDate <= end && r.EndDate >= start)
            {
                overlapping.Add(r);
            }
        }

        return Ok(overlapping);
    }
}
