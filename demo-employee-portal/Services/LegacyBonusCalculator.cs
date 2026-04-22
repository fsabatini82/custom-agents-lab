using EmployeePortal.Models;

namespace EmployeePortal.Services;

[Obsolete("Replaced by EmployeeService.CalculateAnnualBonus. Scheduled for removal in v2.")]
public class LegacyBonusCalculator
{
    public decimal ComputeBonusV1(Employee employee)
    {
        var baseBonus = employee.SalaryGross * 0.05m;

        if (employee.Department == "IT")
        {
            baseBonus *= 1.2m;
        }

        return baseBonus;
    }

    public decimal ComputeBonusV0(Employee employee)
    {
        return employee.SalaryGross * 0.03m;
    }

    public decimal ApplyTaxDeductionV1(decimal amount)
    {
        return amount * 0.72m;
    }

    public decimal ApplyTaxDeductionV2(decimal amount)
    {
        if (amount < 30000) return amount * 0.75m;
        if (amount < 60000) return amount * 0.68m;
        return amount * 0.55m;
    }

    public string FormatCurrencyLegacy(decimal amount)
    {
        return "EUR " + amount.ToString("N2");
    }
}
