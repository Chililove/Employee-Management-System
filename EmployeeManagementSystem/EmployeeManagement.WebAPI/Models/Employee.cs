using System.ComponentModel.DataAnnotations;
public class Employee
{

    public int Id { get; set; }

    [Required]
    public string Name { get; set; }

    [Required]
    public string Position { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary needs to be bigger, please enter a value bigger than 0 for salary")]
    public decimal Salary { get; set; }


}