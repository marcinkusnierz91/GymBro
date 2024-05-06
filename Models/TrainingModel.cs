using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace GymBro.Models;

public class TrainingModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public int UserId { get; set; }
    [Required]
    public DateTime TrainingDate { get; set; }
    public UserModel? User { get; set; }
    public List<ExerciseModel>? Exercises { get; set; }
    public List<ExerciseSeriesModel>? ExerciseSeries { get; set; }
}

public class TraingModelValidator : AbstractValidator<TrainingModel>
{
    public TraingModelValidator()
    {
        RuleFor(x => x.Id)
            .NotNull();
    }
}