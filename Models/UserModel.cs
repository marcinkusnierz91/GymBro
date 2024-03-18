using System.ComponentModel.DataAnnotations;

namespace GymBro.Models;

public class UserModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public string Last_name { get; set; }
    [Required]
    public string Email { get; set; }
    [Required]
    public string Password { get; set; }
    public DateTime Date_created { get; set; } = DateTime.Now;
}