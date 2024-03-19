using System.ComponentModel.DataAnnotations;

namespace GymBro.Models;

public class TrainingModel
{
    [Key]
    public int Id { get; set; }
    public int UserId { get; set; }
    public DateTime TrainingDate { get; set; }
    public UserModel User { get; set; } 
}