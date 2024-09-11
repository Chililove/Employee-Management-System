using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using EmployeeManagement.ConsoleApp.Models;


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
        while(true)
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