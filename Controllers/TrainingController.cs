using GymBro.Data;
using GymBro.Models;
using Microsoft.AspNetCore.Mvc;

namespace GymBro.Controllers;

public class TrainingController : Controller
{
    private readonly ApplicationDbContext _db;

    public TrainingController(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public IActionResult Index()
    {
        // var objTrainingList = _db.Trainings.ToList();
        IEnumerable<TrainingModel> objTrainingList = _db.Trainings;
        return View(objTrainingList);
    }
}