using System.ComponentModel.DataAnnotations;

namespace GymBro.Models;

public class ExerciseModel
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string ExerciseName { get; set; }
    public int TrainingId { get; set; }
    public TrainingModel Training { get; set; }
}