using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : ControllerBase
{
    private static List<Employee> Employees = new List<Employee>

    {
                new Employee { Id = 1, Name = "John Doe", Position = "Developer", Salary = 50000 },
        new Employee { Id = 2, Name = "Jane Smith", Position = "Manager", Salary = 70000 }

    };
    //GET
    [HttpGet]
    public ActionResult<IEnumerable<Employee>> Get(int pageNumber = 1, int pageSize = 10)
    {
        try
        {
            Console.WriteLine("Received the GET request for page number {pageNumber} with this page size {pageSize}.");
            var pagedEmployees = Employees
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToList();

            if (!pagedEmployees.Any())
            {
                return NotFound("There is no employees on this page");
            }
            return Ok(pagedEmployees);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There is an error in GET request: {ex.Message}");
            return StatusCode(500, "Request was not processed because of error");
        }
    }
    //POST
    [HttpPost]
    public IActionResult Post(Employee employee)
    {
        try
        {
            Console.WriteLine("POST request is received");
            if (employee == null)
            {
                Console.WriteLine("The employee data is not valid");
                return BadRequest("Requires employee data");
            }

            employee.Id = Employees.Count + 1;
            Employees.Add(employee);
            Console.WriteLine($"Succesfully added the employee: {employee.Name}");
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"POST request has an error: {ex.Message}");
            return StatusCode(500, "Error in when adding employee");
        }
    }

    //PUT
    [HttpPut("{id}")]
    public IActionResult Put(int id, Employee updatedEmployee)
    {
        try
        {
            Console.WriteLine($"The Employee id is received succesfully: {id}");
            var employee = Employees.FirstOrDefault(employee => employee.Id == id);
            if (employee == null)
            {
                Console.WriteLine("Cannot find the employee requested.");
                return NotFound("Cannot find the employee requested");
            }

            employee.Name = updatedEmployee.Name;
            employee.Position = updatedEmployee.Position;
            employee.Salary = updatedEmployee.Salary;
            Console.WriteLine($"Updating the employee was succesful for: {employee.Name}");
            return NoContent();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"The PUT request failed: {ex.Message}");
            return StatusCode(500, "The was an error when updating with PUT reguest on the employee");
        }
    }

    //DELETE
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        try
        {
            Console.WriteLine($"The employee with id was deleted: {id}");
            var employee = Employees.FirstOrDefault(e => e.Id == id);
            if (employee == null)
            {
                Console.WriteLine("Could not find the employee to delete");
                return NotFound("Could not find the employee to delete");
            }

            Employees.Remove(employee);
            Console.WriteLine($"The employee was deleted: {employee.Name}");
            return NoContent();
        }
        catch (Exception ex)
        {

            Console.WriteLine($"DELTE Request error: {ex.Message}");
            return StatusCode(500, "Error while deleting employee");
        }

    }




    //search functionality to get employees by name or position
    [HttpGet("search")]
    public ActionResult<IEnumerable<Employee>> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
        {
            Console.WriteLine("Empty search is recieved.");
            return BadRequest("Query should be empty.");
        }
        var result = Employees.Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase) || e.Position.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

        if (!result.Any())
        {
            Console.WriteLine("No match found.");
            return NotFound("No employees found matching the search terms.");
        }
        Console.WriteLine("The search filterd the data.");
        return Ok(result);


    }

    [HttpGet("sorted")]

    public ActionResult<IEnumerable<Employee>> GetSorted(string sortBy = "Name", bool descending = false)
    {
        try
        {
            Console.WriteLine($"GET request for sort by {sortBy}, descending: {descending}.");
            var sortedEmployees = Employees.AsQueryable(); //gets all

            switch (sortBy.ToLower()) //switch statement to use on sortBy param

            {
                case "name":
                    sortedEmployees = descending ? sortedEmployees.OrderByDescending(e => e.Name) : sortedEmployees.OrderBy(e => e.Name);
                    break;
                case "position":
                    sortedEmployees = descending ? sortedEmployees.OrderByDescending(e => e.Position) : sortedEmployees.OrderBy(e => e.Position);
                    break;
                case "salary":
                    sortedEmployees = descending ? sortedEmployees.OrderByDescending(e => e.Salary) : sortedEmployees.OrderBy(e => e.Salary);
                    break;
                default:
                    return BadRequest("Sorting needs either a name, position or salary to sort");
            }
            return Ok(sortedEmployees.ToList());
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error while sorting: {ex.Message}");
            return StatusCode(500, "Error while sorting employee data");
        }
    }

    //for validation i used data annotaions to make sure the requests are validated, in the models file


}