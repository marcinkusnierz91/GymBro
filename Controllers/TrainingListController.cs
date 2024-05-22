using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
                .ToList();

            // Pobierz listę lat, w których odbywały się treningi.
            ViewBag.AvailableYears = await _context.Trainings
                .Select(t => t.TrainingDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            return View(groupedByYear);
        }

        // GET: TrainingList/FilterByYear
        public async Task<IActionResult> FilterByYear(int year)
        {
            var trainingsForYear = await _context.Trainings
                .Where(t => t.TrainingDate.Year == year)
                .ToListAsync();

            var groupedByMonth = trainingsForYear
                .GroupBy(t => t.TrainingDate.Month)
                .ToList();

            ViewBag.SelectedYear = year;
            // Upewnij się, że lista lat jest dostępna również po filtrowaniu.
            ViewBag.AvailableYears = await _context.Trainings
                .Select(t => t.TrainingDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            return View("Index", groupedByMonth);
        }

        private bool TrainingModelExists(int id)
        {
            return _context.Trainings.Any(e => e.Id == id);
        }
    }
}
