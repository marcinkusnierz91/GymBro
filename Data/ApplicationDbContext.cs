using GymBro.Models;
using Microsoft.EntityFrameworkCore;

namespace GymBro.Data;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
    { }
    
    public DbSet<UserModel> Users { get; set; }
}