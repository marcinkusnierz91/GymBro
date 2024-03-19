using System.ComponentModel.DataAnnotations;

namespace GymBro.Models;
// seria cwiczen po polskiemu
public class ExerciseSeriesModel
{
    [Key]
    public int Id { get; set; }
    public int ExerciseId { get; set; }
    public int SeriesAmount { get; set; }
    public ExerciseModel Exercise { get; set; }
    
}