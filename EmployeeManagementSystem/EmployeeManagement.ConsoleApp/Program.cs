using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using EmployeeManagement.ConsoleApp.Models;
using System.Diagnostics;


public class Program
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task Main(string[] args)
    {
        try
        {

            Console.WriteLine("Console App for Employee Management System");
            Console.WriteLine("------------------------------------------");

            // created interface
            await ShowMenuAsync();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There has been an error: {ex.Message}");
        }
    }

    private static async Task ShowMenuAsync()
    {
        while (true)
        {
            Console.WriteLine("\nSelect action:");
            Console.WriteLine("1. Fetching all employees");
            Console.WriteLine("2. Searching for employees");
            Console.WriteLine("3. Exporting employees to csv file");
            Console.WriteLine("4. Quit");
            Console.WriteLine("Select an option: ");

            var choice = Console.ReadLine();
            switch (choice)
            {
                case "1":
                    await FetchEmployeesAsync();
                    break;
                case "2":
                    await SearchEmployeesAsync();
                    break;
                case "3":
                    await FetchAndExportEmployeesAsync();
                    break;
                case "4":
                    Console.WriteLine("Exiting...");
                    return;
                default:
                    Console.WriteLine("Your choice is not valid, please select another option.");
                    break;

            }

        }
    }

    private static async Task FetchEmployeesAsync()
    {
        string apiUrl = "http://localhost:5050/api/employees";

        try
        {
            var employees = await client.GetFromJsonAsync<List<Employee>>(apiUrl);
            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No data found on employee");
                return;
            }
            Console.WriteLine("Employee data grabbed succesfully");
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id}: {employee.Name} - {employee.Position} - {employee.Salary}");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error Occured see: {ex.Message}");

        }
    }

    private static async Task SearchEmployeesAsync()
    {
        Console.WriteLine("Enter search term (like name or position): ");
        var searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Search term is not valid.");
            return;

        }

        string apiUrl = $"http://localhost:5050/api/employees/search?query={searchTerm}";

        try
        {
            var employees = await client.GetFromJsonAsync<List<Employee>>(apiUrl);
            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("There is no employees matching this search");
                return;
            }
            Console.WriteLine("Search results:");
            foreach (var employee in employees)
            {
                Console.WriteLine($"{employee.Id}: {employee.Name} - {employee.Position} - {employee.Salary}");
            }
        }
        catch (HttpRequestException httpEx)
        {
            Console.WriteLine($"HTTP error: {httpEx.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error accured: {ex.Message}");
        }

    }


    private static void ExportToCsv(List<Employee> employees)
    {
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "employees.csv");

        using (var writer = new StreamWriter("employees.csv"))
        {
            writer.WriteLine("Id,Name,Position,Salary");
            foreach (var employee in employees)
            {
                writer.WriteLine($"{employee.Id}, {employee.Name}, {employee.Position}, {employee.Salary}");
            }
        }
        Console.WriteLine($"Employee data is exported to: {filePath}");
    }

}