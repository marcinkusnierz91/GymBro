using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GymBro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(int? monthOffset = 0)
        {
            // Pobierz dni tygodnia oraz numery dni miesiąca dla bieżącego lub wybranego miesiąca
            Dictionary<string, int> daysOfWeekAndMonthDay = GetDaysOfWeekAndMonthDayForMonthOffset(monthOffset ?? 0);

            ViewBag.DaysOfWeekAndMonthDay = daysOfWeekAndMonthDay; // Przekazuj listę dni tygodnia i numerów dni miesiąca za pomocą ViewBag
            string monthName = DateTime.Now.AddMonths(monthOffset ?? 0).ToString("MMMM yyyy",CultureInfo.GetCultureInfo("pl-PL"));
            ViewBag.DisplayedMonth = char.ToUpper(monthName[0]) + monthName.Substring(1);
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
        

    }
}
