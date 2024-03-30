using GymBro.Data;
using GymBro.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace GymBro.Controllers
{
    public class TraininggController : Controller
    {
        private readonly ApplicationDbContext _db;

        public TraininggController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index(string dayOfWeek, string dayOfMonth)
        {
            // Przekazanie informacji o wybranym dniu do widoku
            ViewBag.SelectedDayOfWeek = dayOfWeek;
            ViewBag.SelectedDayOfMonth = dayOfMonth;

            // Jeśli została wybrana data, pobierz treningi tylko dla tej daty
            if (!string.IsNullOrEmpty(dayOfWeek) && !string.IsNullOrEmpty(dayOfMonth))
            {
                // Pobieranie nazwy dnia tygodnia w języku angielskim
                DayOfWeek parsedDayOfWeek;
                switch (dayOfWeek.ToLower())
                {
                    case "poniedziałek":
                        parsedDayOfWeek = DayOfWeek.Monday;
                        break;
                    case "wtorek":
                        parsedDayOfWeek = DayOfWeek.Tuesday;
                        break;
                    case "środa":
                        parsedDayOfWeek = DayOfWeek.Wednesday;
                        break;
                    case "czwartek":
                        parsedDayOfWeek = DayOfWeek.Thursday;
                        break;
                    case "piątek":
                        parsedDayOfWeek = DayOfWeek.Friday;
                        break;
                    case "sobota":
                        parsedDayOfWeek = DayOfWeek.Saturday;
                        break;
                    case "niedziela":
                        parsedDayOfWeek = DayOfWeek.Sunday;
                        break;
                    default:
                        // Jeśli nazwa dnia tygodnia jest nieprawidłowa, zwróć pustą listę treningów
                        return View(new List<TrainingModel>());
                }

                // Parsowanie numeru dnia miesiąca
                if (!int.TryParse(dayOfMonth, out int parsedDayOfMonth))
                {
                    // Jeśli numer dnia miesiąca nie jest prawidłowy, zwróć pustą listę treningów
                    return View(new List<TrainingModel>());
                }

                // Tworzenie daty na podstawie przekazanych danych
                DateTime selectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, parsedDayOfMonth);
                while (selectedDate.DayOfWeek != parsedDayOfWeek)
                {
                    selectedDate = selectedDate.AddDays(1);
                }

                // Pobieranie treningów dla wybranej daty
                var objTrainingList = _db.Trainings
                    .Include(t => t.Exercises)
                    .Where(t => t.TrainingDate.Date == selectedDate.Date)
                    .ToList();
                return View(objTrainingList);
            }
            // Jeśli data nie została wybrana, zwróć pustą listę treningów
            else
            {
                return View(new List<TrainingModel>());
            }
        }

        // Akcja do tworzenia nowego treningu
        public IActionResult CreateTraining()
        {
            // Tworzenie nowego obiektu treningu
            TrainingModel newTraining = new TrainingModel
            {
                UserId = 1, // Identyfikator użytkownika, możesz ustawić odpowiednio dla swojej aplikacji
                TrainingDate = DateTime.Now // Aktualna data jako data treningu
            };

            // Dodawanie nowego treningu do bazy danych
            _db.Trainings.Add(newTraining);
            _db.SaveChanges();

            // Przekierowanie użytkownika do akcji Index po utworzeniu treningu
            return RedirectToAction("Index");
        }
    }
}
