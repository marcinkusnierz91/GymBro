using System.ComponentModel.DataAnnotations;

namespace GymBro.Models;

public class RepetitionModel
{
    [Key]
    public int Id { get; set; }
    public int RepetitionsAmount { get; set; }
    public int SeriesId { get; set; }
    public ExerciseSeriesModel ExerciseSeries { get; set; }
}