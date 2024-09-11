using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private static List<Employee> Employees = new List<Employee>();

    [HttpGet]
    public ActionResult<IEnumerable<Employee>> Get() { return Employees; }

    [HttpPost]
    public IActionResult Post(Employee employee)
    {
        Console.WriteLine("GET request received");

        employee.Id = Employees.Count + 1;
        Employees.Add(employee);
        return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
    }

    [HttpPut("{id}")]
    public IActionResult Put(int id, Employee updatedEmployee)
    {
        var employee = Employees.FirstOrDefault(employee => employee.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        employee.Name = updatedEmployee.Name;
        employee.Position = updatedEmployee.Position;
        employee.Salary = updatedEmployee.Salary;
        return NoContent();
    }
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        var employee = Employees.FirstOrDefault(e => e.Id == id);
        if (employee == null)
        {
            return NotFound();
        }
        Employees.Remove(employee);
        return NoContent();
    }

}