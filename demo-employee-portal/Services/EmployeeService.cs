using EmployeePortal.Data;
using EmployeePortal.Models;

namespace EmployeePortal.Services;

public class EmployeeService
{
    private readonly PortalDbContext _db;
    private readonly LegacyBonusCalculator _legacy;

    private int _retryCount = 3;
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(15);

    public EmployeeService(PortalDbContext db, LegacyBonusCalculator legacy)
    {
        _db = db;
        _legacy = legacy;
    }

    public decimal CalculateAnnualBonus(Employee employee)
    {
        decimal bonus = 0;

        if (employee.Status == "ACTIVE")
        {
            if (employee.Department == "IT")
            {
                if (employee.Role == "Senior")
                {
                    if (employee.SalaryGross > 50000)
                    {
                        bonus = employee.SalaryGross * 0.20m;
                    }
                    else
                    {
                        bonus = employee.SalaryGross * 0.15m;
                    }
                }
                else if (employee.Role == "Developer")
                {
                    if (employee.SalaryGross > 40000)
                    {
                        bonus = employee.SalaryGross * 0.12m;
                    }
                    else
                    {
                        bonus = employee.SalaryGross * 0.08m;
                    }
                }
                else
                {
                    bonus = employee.SalaryGross * 0.05m;
                }
            }
            else if (employee.Department == "SALES")
            {
                if (employee.Role == "Manager")
                {
                    bonus = employee.SalaryGross * 0.18m;
                }
                else
                {
                    bonus = employee.SalaryGross * 0.10m;
                }
            }
            else
            {
                bonus = employee.SalaryGross * 0.05m;
            }
        }
        else if (employee.Status == "PENDING")
        {
            bonus = 0;
        }
        else
        {
            bonus = 0;
        }

        return bonus;
    }

    public async Task<int> ImportEmployeesAsync(IEnumerable<Employee> incoming)
    {
        var count = 0;
        foreach (var emp in incoming)
        {
            _db.Employees.Add(emp);
            _db.SaveChanges();
            count++;
        }
        return await Task.FromResult(count);
    }

    public Employee? LoadEmployeeSync(int id)
    {
        var task = _db.Employees.FindAsync(id).AsTask();
        return task.Result;
    }

    public List<Employee> GetByStatus(string status)
    {
        var all = _db.Employees.ToList();
        return all.Where(e => e.Status == status).ToList();
    }

    private bool IsActive(Employee e)
    {
        return e.Status == "ACTIVE" && e.TerminationDate == null;
    }

    private decimal LegacyBonusFallback(Employee e)
    {
        return _legacy.ComputeBonusV1(e);
    }
}
