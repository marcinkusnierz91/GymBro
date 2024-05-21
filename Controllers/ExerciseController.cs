using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBro.Data;
using GymBro.Models;

namespace GymBro.Controllers
{
    public class ExerciseController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ExerciseController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Exercise
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Exercises.Include(e => e.Training);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Exercise/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseModel = await _context.Exercises
                .Include(e => e.Training)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseModel == null)
            {
                return NotFound();
            }

            return View(exerciseModel);
        }

        // GET: Exercise/Create
        public IActionResult Create()
        {
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Id");
            return View();
        }

        // POST: Exercise/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(string dayOfMonth, int month, string year, [Bind("Id,ExerciseName,TrainingId")] ExerciseModel exerciseModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(exerciseModel);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Training", new { dayOfMonth = dayOfMonth, month = month, year = year });
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Id", exerciseModel.TrainingId);
            return RedirectToAction("Index", new { dayOfMonth = dayOfMonth, month = month, year = year });
        }

        // GET: Exercise/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseModel = await _context.Exercises.FindAsync(id);
            if (exerciseModel == null)
            {
                return NotFound();
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Id", exerciseModel.TrainingId);
            return View(exerciseModel);
        }

        // POST: Exercise/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseName,TrainingId")] ExerciseModel exerciseModel)
        {
            if (id != exerciseModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseModelExists(exerciseModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["TrainingId"] = new SelectList(_context.Trainings, "Id", "Id", exerciseModel.TrainingId);
            return View(exerciseModel);
        }

        // GET: Exercise/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseModel = await _context.Exercises
                .Include(e => e.Training)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseModel == null)
            {
                return NotFound();
            }

            return View(exerciseModel);
        }

        // POST: Exercise/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseModel = await _context.Exercises.FindAsync(id);
            if (exerciseModel != null)
            {
                _context.Exercises.Remove(exerciseModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseModelExists(int id)
        {
            return _context.Exercises.Any(e => e.Id == id);
        }

        [HttpPost]
        public async Task<IActionResult> AddExerciseToTraining(string exerciseName, int trainingId, int dayOfMonth, int month, int year)
        {
            if (string.IsNullOrEmpty(exerciseName) || trainingId <= 0)
            {
                return BadRequest("Invalid input data.");
            }

            var exercise = new ExerciseModel
            {
                ExerciseName = exerciseName,
                ExerciseDate = DateTime.Now,
                TrainingId = trainingId
            };

            _context.Add(exercise);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Training", new { dayOfMonth = dayOfMonth, month = month, year = year });
        }

        [HttpGet]
        public IActionResult SelectMusclePart(int trainingId, int dayOfMonth, int month, int year)
        {
            ViewBag.TrainingId = trainingId;
            ViewBag.DayOfMonth = dayOfMonth;
            ViewBag.Month = month;
            ViewBag.Year = year;
            return View();
        }

        public IActionResult SelectExercise(string exerciseType, int trainingId, int dayOfMonth, int month, int year)
        {
            var exercises = new Dictionary<string, List<string>>
            {
                { "Chest", new List<string> { "Bench Press", "Incline Bench Press", "Chest Fly", "Push Ups" } },
                { "Back", new List<string> { "Pull Ups", "Deadlift", "Bent Over Row" } },
                { "Shoulders", new List<string> { "Shoulder Press", "Lateral Raise", "Front Raise" } },
                { "Biceps", new List<string> { "Bicep Curl", "Hammer Curl", "Concentration Curl" } },
                { "Triceps", new List<string> { "Tricep Extension", "Skull Crushers", "Tricep Dips" } },
                { "Legs", new List<string> { "Squats", "Leg Press", "Lunges" } }
            };

            var selectedExercises = exercises.ContainsKey(exerciseType) ? exercises[exerciseType] : new List<string>();

            ViewBag.TrainingId = trainingId;
            ViewBag.DayOfMonth = dayOfMonth;
            ViewBag.Month = month;
            ViewBag.Year = year;

            return View(selectedExercises);
        }
    }
}
