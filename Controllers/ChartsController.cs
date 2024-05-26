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
    public class ChartsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ChartsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Charts
        public async Task<IActionResult> Index()
        {
            var trainings = await _context.Trainings
                .Include(t => t.Exercises)
                .OrderBy(t => t.TrainingDate)
                .ToListAsync();

            var groupedByYear = trainings
                .GroupBy(t => t.TrainingDate.Year)
                .ToList();

            ViewBag.AvailableYears = await _context.Trainings
                .Select(t => t.TrainingDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            ViewBag.AvailableExercises = await _context.Exercises
                .Select(e => e.ExerciseName)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            ViewBag.AvailableMuscleParts = await _context.Exercises
                .Select(e => e.MusclePart)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            return View(groupedByYear);
        }

        // GET: Charts/Filter
        public async Task<IActionResult> Filter(int? year, string exercise, string musclePart)
        {
            var trainingsQuery = _context.Trainings
                .Include(t => t.Exercises)
                .AsQueryable();

            if (year.HasValue)
            {
                trainingsQuery = trainingsQuery.Where(t => t.TrainingDate.Year == year.Value);
            }

            if (!string.IsNullOrEmpty(exercise))
            {
                trainingsQuery = trainingsQuery.Where(t => t.Exercises.Any(e => e.ExerciseName == exercise));
            }

            if (!string.IsNullOrEmpty(musclePart))
            {
                trainingsQuery = trainingsQuery.Where(t => t.Exercises.Any(e => e.MusclePart == musclePart));
            }

            var trainings = await trainingsQuery.ToListAsync();

            var groupedByYear = trainings
                .GroupBy(t => t.TrainingDate.Year)
                .ToList();

            ViewBag.SelectedYear = year;
            ViewBag.SelectedExercise = exercise;
            ViewBag.SelectedMusclePart = musclePart;

            ViewBag.AvailableYears = await _context.Trainings
                .Select(t => t.TrainingDate.Year)
                .Distinct()
                .OrderBy(y => y)
                .ToListAsync();

            ViewBag.AvailableExercises = await _context.Exercises
                .Select(e => e.ExerciseName)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            ViewBag.AvailableMuscleParts = await _context.Exercises
                .Select(e => e.MusclePart)
                .Distinct()
                .OrderBy(e => e)
                .ToListAsync();

            return View("Index", groupedByYear);
        }
    }
}
