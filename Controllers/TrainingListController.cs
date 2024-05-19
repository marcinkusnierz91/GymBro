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
    public class TrainingListController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingListController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingList
        public async Task<IActionResult> Index()
        {
            var trainings = await _context.Trainings
                .OrderBy(t => t.TrainingDate)
                .ToListAsync();

            var groupedByYear = trainings
                .GroupBy(t => t.TrainingDate.Year)
                .ToList(); // To utworzy listę grup IGrouping<int, TrainingModel>

            return View(groupedByYear); // Przekazujemy listę grup do widoku
        }

        // GET: TrainingList/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingModel = await _context.Trainings
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingModel == null)
            {
                return NotFound();
            }

            return View(trainingModel);
        }

        // GET: TrainingList/Create
        public IActionResult Create()
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: TrainingList/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TrainingDate")] TrainingModel trainingModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(trainingModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", trainingModel.UserId);
            return View(trainingModel);
        }

        // GET: TrainingList/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingModel = await _context.Trainings.FindAsync(id);
            if (trainingModel == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", trainingModel.UserId);
            return View(trainingModel);
        }

        // POST: TrainingList/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,TrainingDate")] TrainingModel trainingModel)
        {
            if (id != trainingModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(trainingModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TrainingModelExists(trainingModel.Id))
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", trainingModel.UserId);
            return View(trainingModel);
        }

        // GET: TrainingList/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trainingModel = await _context.Trainings
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingModel == null)
            {
                return NotFound();
            }

            return View(trainingModel);
        }

        // POST: TrainingList/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingModel = await _context.Trainings.FindAsync(id);
            if (trainingModel != null)
            {
                _context.Trainings.Remove(trainingModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TrainingModelExists(int id)
        {
            return _context.Trainings.Any(e => e.Id == id);
        }
    }
}
