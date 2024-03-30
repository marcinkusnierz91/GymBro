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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
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
    }
}
