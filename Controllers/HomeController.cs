using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using GymBro.Data;

namespace GymBro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _context = db;
        }

        public IActionResult Index(int? monthOffset = 0)
        {
            // Pobierz dni tygodnia oraz numery dni miesiąca dla bieżącego lub wybranego miesiąca
            Dictionary<string, int> daysOfWeekAndMonthDay = GetDaysOfWeekAndMonthDayForMonthOffset(monthOffset ?? 0);

            // Pobierz listę dat treningów dla bieżącego lub wybranego miesiąca
            var trainingDates = GetTrainingDatesForMonthOffset(monthOffset ?? 0);

            // Pobierz nazwę miesiąca i rok
            string monthName = DateTime.Now.AddMonths(monthOffset ?? 0).ToString("MMMM", CultureInfo.GetCultureInfo("pl-PL"));
            int monthNumber = DateTime.Now.AddMonths(monthOffset ?? 0).Month;
            string year = DateTime.Now.AddMonths(monthOffset ?? 0).Year.ToString();

            ViewBag.DisplayedMonth = char.ToUpper(monthName[0]) + monthName.Substring(1);
            ViewBag.MonthNumber = monthNumber;
            ViewBag.DisplayedYear = year;

            ViewBag.DaysOfWeekAndMonthDay = daysOfWeekAndMonthDay; // Przekazuj listę dni tygodnia i numerów dni miesiąca za pomocą ViewBag
            ViewBag.TrainingDates = trainingDates; // Przekazuj listę dat treningów za pomocą ViewBag

            ViewBag.PreviousMonthOffset = monthOffset - 1; // Offset dla poprzedniego miesiąca
            ViewBag.NextMonthOffset = monthOffset + 1; // Offset dla następnego miesiąca
        
            return View();
        }

        private Dictionary<string, int> GetDaysOfWeekAndMonthDayForMonthOffset(int offset)
        {
            Dictionary<string, int> daysOfWeekAndMonthDay = new Dictionary<string, int>();
            DateTime currentDate = DateTime.Now.AddMonths(offset);
            int daysInMonth = DateTime.DaysInMonth(currentDate.Year, currentDate.Month);
            
            // Pętla przez dni w miesiącu z uwzględnieniem przesunięcia
            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime currentDay = new DateTime(currentDate.Year, currentDate.Month, i);
                string dayOfWeek = currentDay.ToString("dddd", CultureInfo.GetCultureInfo("pl-PL")); // Nazwa dnia tygodnia
                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
                
                int monthDay = i; // Numer dnia miesiąca
                string key = $"{dayOfWeek} {monthDay}"; // Klucz łączący nazwę dnia tygodnia i numer dnia miesiąca
                daysOfWeekAndMonthDay.Add(key, monthDay); // Dodaj parę nazwa dnia tygodnia - numer dnia miesiąca do słownika
            }

            return daysOfWeekAndMonthDay;
        }

        private List<DateTime> GetTrainingDatesForMonthOffset(int offset)
        {
            DateTime startDate = DateTime.Now.AddMonths(offset).AddDays(-DateTime.Now.Day + 1).Date;
            DateTime endDate = startDate.AddMonths(1).AddDays(-1); // Ostatni dzień bieżącego lub wybranego miesiąca

            // Pobierz listę dat treningów dla okresu od pierwszego do ostatniego dnia miesiąca
            var trainingDates = _context.Trainings
                .Where(t => t.TrainingDate >= startDate && t.TrainingDate <= endDate)
                .Select(t => t.TrainingDate.Date)
                .Distinct()
                .ToList();

            return trainingDates;
        }
    }
}