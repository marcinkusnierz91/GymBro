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

        public IActionResult Index(int? month = null, int? year = null)
        {
            DateTime currentDate;
            if (month.HasValue && year.HasValue)
            {
                currentDate = new DateTime(year.Value, month.Value, 1);
            }
            else
            {
                currentDate = DateTime.Now;
            }

            Dictionary<string, int> daysOfWeekAndMonthDay = GetDaysOfWeekAndMonthDay(currentDate);
            var trainingDates = GetTrainingDates(currentDate);

            string monthName = currentDate.ToString("MMMM", CultureInfo.GetCultureInfo("en-US"));
            int monthNumber = currentDate.Month;
            string displayedYear = currentDate.Year.ToString();

            ViewBag.DisplayedMonth = char.ToUpper(monthName[0]) + monthName.Substring(1);
            ViewBag.MonthNumber = monthNumber;
            ViewBag.DisplayedYear = displayedYear;

            ViewBag.DaysOfWeekAndMonthDay = daysOfWeekAndMonthDay;
            ViewBag.TrainingDates = trainingDates;

            DateTime previousMonthDate = currentDate.AddMonths(-1);
            ViewBag.PreviousMonth = previousMonthDate.Month;
            ViewBag.PreviousYear = previousMonthDate.Year;

            DateTime nextMonthDate = currentDate.AddMonths(1);
            ViewBag.NextMonth = nextMonthDate.Month;
            ViewBag.NextYear = nextMonthDate.Year;

            return View();
        }

        private Dictionary<string, int> GetDaysOfWeekAndMonthDay(DateTime date)
        {
            Dictionary<string, int> daysOfWeekAndMonthDay = new Dictionary<string, int>();
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            for (int i = 1; i <= daysInMonth; i++)
            {
                DateTime currentDay = new DateTime(date.Year, date.Month, i);
                string dayOfWeek = currentDay.ToString("dddd", CultureInfo.GetCultureInfo("en-US"));
                dayOfWeek = char.ToUpper(dayOfWeek[0]) + dayOfWeek.Substring(1);
                daysOfWeekAndMonthDay.Add($"{dayOfWeek} {i}", i);
            }

            return daysOfWeekAndMonthDay;
        }

        private List<DateTime> GetTrainingDates(DateTime date)
        {
            DateTime startDate = new DateTime(date.Year, date.Month, 1);
            DateTime endDate = startDate.AddMonths(1).AddDays(-1);

            var trainingDates = _context.Trainings
                .Where(t => t.TrainingDate >= startDate && t.TrainingDate <= endDate)
                .Select(t => t.TrainingDate.Date)
                .Distinct()
                .ToList();

            return trainingDates;
        }
    }
}
