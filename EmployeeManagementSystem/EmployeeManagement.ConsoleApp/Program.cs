using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using EmployeeManagement.ConsoleApp;


public class Program
{
    private static readonly HttpClient client = new HttpClient();

    public static async Task Main(string[] args)
    {
        try
        {
            string apiUrl = "http://localhost:5050/api/employess";

            Console.WriteLine("Getting data from my web api for employees");

            // GET
            var employees = await client.GetFromJsonAsync<List<Employee>>(apiUrl);

            if (employees == null || employees.Count == 0)
            {
                Console.WriteLine("No data of employees are found.");
                return;
            }

            Console.WriteLine("Succesfully got employee data grabbed/fetched");

            //Export CSV file
            // ExportToCsv(employees);

            Console.WriteLine("Exporting data from employees to csv file");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"There has been an error: {ex.Message}");
        }
    }
    private static void ExportToCsv(List<Employee> employees)
    {
        using (var writer = new StreamWriter("employees.csv"))
        {
            writer.WriteLine("Id,Name,Position,Salary");
            foreach (var employee in employees)
            {
                writer.WriteLine($"{employee.Id}, {employee.Name}, {employee.Position}, {employee.Salary}");
            }
        }
    }

}