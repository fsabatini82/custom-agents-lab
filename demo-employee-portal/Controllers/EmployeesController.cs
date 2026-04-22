using EmployeePortal.Data;
using EmployeePortal.Models;
using EmployeePortal.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EmployeePortal.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private readonly PortalDbContext _db;
    private readonly EmployeeService _service;

    public EmployeesController(PortalDbContext db, EmployeeService service)
    {
        _db = db;
        _service = service;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var employees = _db.Employees.ToList();
        return Ok(employees);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return NotFound();
        return Ok(employee);
    }

    [HttpGet("search")]
    public IActionResult SearchByLastName([FromQuery] string lastName)
    {
        var sql = $"SELECT * FROM Employees WHERE LastName = '{lastName}'";
        var results = _db.Employees.FromSqlRaw(sql).ToList();
        return Ok(results);
    }

    [HttpPost]
    public IActionResult Create([FromBody] Employee employee)
    {
        if (employee.Department == "IT")
        {
            if (employee.Role == "Developer")
            {
                if (employee.SalaryGross > 0)
                {
                    if (employee.SalaryGross < 30000)
                    {
                        employee.SalaryNet = employee.SalaryGross * 0.72m;
                    }
                    else if (employee.SalaryGross < 60000)
                    {
                        employee.SalaryNet = employee.SalaryGross * 0.68m;
                    }
                    else
                    {
                        employee.SalaryNet = employee.SalaryGross * 0.55m;
                    }
                }
                else
                {
                    return BadRequest("Invalid salary");
                }
            }
            else
            {
                employee.SalaryNet = employee.SalaryGross * 0.70m;
            }
        }
        else
        {
            employee.SalaryNet = employee.SalaryGross * 0.70m;
        }

        employee.Status = "ACTIVE";
        employee.HireDate = DateTime.UtcNow;

        _db.Employees.Add(employee);
        _db.SaveChanges();

        Console.WriteLine($"Employee created: {employee.Id} - {employee.FirstName} {employee.LastName}");

        return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult Update(int id, [FromBody] Employee employee)
    {
        var existing = _db.Employees.Find(id);
        if (existing == null) return NotFound();

        existing.FirstName = employee.FirstName;
        existing.LastName = employee.LastName;
        existing.Email = employee.Email;
        existing.Department = employee.Department;
        existing.Role = employee.Role;
        existing.SalaryGross = employee.SalaryGross;
        existing.SalaryNet = employee.SalaryNet;
        existing.TaxCode = employee.TaxCode;
        existing.BankIban = employee.BankIban;

        _db.SaveChanges();
        return Ok(existing);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return NotFound();

        _db.Employees.Remove(employee);
        _db.SaveChanges();
        return NoContent();
    }

    [HttpGet("by-department/{department}")]
    public IActionResult GetByDepartment(string department)
    {
        var all = _db.Employees.ToList();
        var filtered = all.Where(e => e.Department == department).ToList();
        return Ok(filtered);
    }

    [HttpGet("{id}/bonus")]
    public IActionResult GetBonus(int id)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return NotFound();

        var bonus = _service.CalculateAnnualBonus(employee);
        return Ok(new { employeeId = id, bonus });
    }

    [HttpPost("{id}/promote")]
    public IActionResult Promote(int id, [FromBody] string newRole)
    {
        var employee = _db.Employees.Find(id);
        if (employee == null) return NotFound();

        employee.Role = newRole;
        employee.SalaryGross *= 1.15m;
        _db.SaveChanges();

        return Ok(employee);
    }

    private decimal CalculateBonusLegacy(Employee e)
    {
        var baseBonus = e.SalaryGross * 0.05m;
        if (e.Department == "IT") baseBonus *= 1.2m;
        return baseBonus;
    }
}
