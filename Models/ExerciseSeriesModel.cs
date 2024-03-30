using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GymBro.Models
{
    // seria ćwiczeń po polskiemu
    public class ExerciseSeriesModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int ExerciseId { get; set; }

        public int SeriesAmount { get; set; }

        public int SeriesNumber { get; set; }

        public int Repetitions { get; set; }

        public int Weight { get; set; }

        public ExerciseModel? Exercise { get; set; }
    }
}