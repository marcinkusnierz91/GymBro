using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FluentValidation;

namespace GymBro.Models;

public class ExerciseModel
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    public string? ExerciseName { get; set; }
    public string? MusclePart { get; set; }
    public DateTime ExerciseDate { get; set; }
    public int TrainingId { get; set; }
    public TrainingModel? Training { get; set; }
    public List<ExerciseSeriesModel>? ExerciseSeries { get; set; }
    // public ExerciseSeriesModel? ExerciseSeries { get; set; }
}

public class ExerciseValidator : AbstractValidator<ExerciseModel>
{
    public ExerciseValidator()
    {
        RuleFor(x => x.Id).NotNull();
    }
}
    
