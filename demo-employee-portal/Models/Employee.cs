namespace EmployeePortal.Models;

public class Employee
{
    public int Id { get; set; }
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Department { get; set; } = "";
    public string Role { get; set; } = "";

    public decimal SalaryGross { get; set; }
    public decimal SalaryNet { get; set; }

    public string TaxCode { get; set; } = "";
    public string BankIban { get; set; } = "";

    public string Status { get; set; } = "ACTIVE";
    public DateTime HireDate { get; set; }
    public DateTime? TerminationDate { get; set; }

    public int? ManagerId { get; set; }
    public Employee? Manager { get; set; }
}
