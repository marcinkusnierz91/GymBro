using GymBro.Models;
using Microsoft.EntityFrameworkCore;

namespace GymBro.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    { }
    
    public DbSet<UserModel> Users { get; set; }
    public DbSet<ExerciseModel> Exercises { get; set; }
    public DbSet<ExerciseSeriesModel> ExercisesSeries { get; set; }
    public DbSet<RepetitionModel> Repetitions { get; set; }
    public DbSet<TrainingModel> Trainings { get; set; }

}