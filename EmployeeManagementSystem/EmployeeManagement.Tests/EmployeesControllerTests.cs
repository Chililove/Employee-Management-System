using Xunit;
using EmployeeManagement.WebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
//using Microsoft.AspNetCore.Authorization.Infrastructure;

public class EmployeesControllerTests
{
    [Fact]
    public void Get_ReturnAllEmployees()
    {
        // arange, act, assert
        var controller = new EmployeesController();

        var result = controller.Get();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<Employee>>>(result);

        if (actionResult.Result is OkObjectResult okResult)
        {
            var employees = Assert.IsType<List<Employee>>(okResult.Value);
            Assert.NotEmpty(employees);
        }
        else if (actionResult.Result is NotFoundObjectResult notFoundResult)
        {
            Assert.IsType<string>(notFoundResult.Value);
        }
        else
        {
            Assert.Fail("Result was neither OkObjectResult nor was it NotFoundObjectResult.");
        }
    }

    [Fact]
    public void Get_NotFoundWhenNoEmployees()
    {
        // arange, act, assert
        var controller = new EmployeesController();
        EmployeesController.Employees.Clear();

        var result = controller.Get();

        var actionResult = Assert.IsType<ActionResult<IEnumerable<Employee>>>(result);
        var notFoundResult = Assert.IsType<NotFoundObjectResult>(actionResult.Result);
    }

    [Fact]
    public void Post_CreatesEmployee_ReturnsActionCreatedAt()
    {
        var controller = new EmployeesController();
        var newEmployee = new Employee { Name = "Janet", Position = "Designer", Salary = 36000 };

        var result = controller.Post(newEmployee);

        var actionResult = Assert.IsType<CreatedAtActionResult>(result);
        var createdEmployee = Assert.IsType<Employee>(actionResult.Value);
        Assert.Equal("Janet", createdEmployee.Name);
        Assert.Equal("Designer", createdEmployee.Position);
        Assert.Equal(36000, createdEmployee.Salary);
        Assert.NotEqual(0, createdEmployee.Id);
    }

    [Fact]
    public void Put_UpdateExistingEmployee_ReturnsNoContent()
    {
        var controller = new EmployeesController();
        EmployeesController.Employees.Clear();

        var existingEmployee = new Employee { Id = 1, Name = "Sami", Position = "Personal Trainer", Salary = 25000 };
        EmployeesController.Employees.Add(existingEmployee);

        var updatedEmployee = new Employee { Name = "Updated Name", Position = "Updated Position", Salary = 40000 };

        var result = controller.Put(existingEmployee.Id, updatedEmployee);

        Assert.IsType<NoContentResult>(result);
        Assert.Equal("Updated Name", existingEmployee.Name);
        Assert.Equal("Updated Position", existingEmployee.Position);
        Assert.Equal(40000, existingEmployee.Salary);
    }
}
