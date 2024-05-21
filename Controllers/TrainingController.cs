using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GymBro.Data;
using GymBro.Models;

namespace GymBro.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrainingController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Training
        public IActionResult Index(string dayOfMonth, int month, string year)
        {
            // Przekazanie informacji o wybranym dniu, miesiącu i roku do widoku
            ViewBag.SelectedDayOfMonth = dayOfMonth;
            ViewBag.DisplayedMonth = month;
            ViewBag.DisplayedYear = year;

            // Jeśli została wybrana data, pobierz treningi tylko dla tej daty
            if (!string.IsNullOrEmpty(dayOfMonth))
            {
                // Parsowanie numeru dnia miesiąca
                if (!int.TryParse(dayOfMonth, out int parsedDayOfMonth))
                {
                    // Jeśli numer dnia miesiąca nie jest prawidłowy, zwróć pustą listę treningów
                    return BadRequest("Invalid day of month.");
                }

                // Tworzenie daty na podstawie przekazanych danych
                DateTime selectedDate = new DateTime(int.Parse(year), month, parsedDayOfMonth);

                // Pobieranie treningów dla wybranej daty wraz z danymi użytkowników
                var objTrainingList = _context.Trainings
                    .Include(t => t.Exercises)!
                    .ThenInclude(e => e.ExerciseSeries) // Załączenie ExerciseSeries dla każdego Exercises
                    .Include(t => t.User) // Załączenie danych użytkownika
                    .Where(t => t.TrainingDate.Date == selectedDate.Date)
                    .ToList();

                return View(objTrainingList);
            }
            // Jeśli data nie została wybrana, zwróć listę treningów dla bieżącego miesiąca
            else
            {
                // Pobierz bieżący miesiąc
                int currentMonth = DateTime.Now.Month;
                int currentYear = DateTime.Now.Year;

                // Pobieranie treningów dla bieżącego miesiąca wraz z danymi użytkowników
                var objTrainingList = _context.Trainings
                    .Include(t => t.Exercises)
                    .Include(t => t.User) // Załączenie danych użytkownika
                    .Include(t => t.ExerciseSeries)
                    .Where(t => t.TrainingDate.Month == currentMonth && t.TrainingDate.Year == currentYear)
                    .ToList();

                return View(objTrainingList);
            }
        }
        
        // GET: Training/Details/5
        public async Task<IActionResult> Details(int? id, string dayOfWeek, string dayOfMonth)
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

            ViewBag.DayOfWeek = dayOfWeek;
            ViewBag.DayOfMonth = dayOfMonth;

            return View(trainingModel);
        }


        // GET: Training/Create
        public IActionResult Create(string dayOfWeek, string dayOfMonth)
        {
            ViewBag.DayOfWeek = dayOfWeek;
            ViewBag.DayOfMonth = dayOfMonth;
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }


        // POST: Training/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,TrainingDate")] TrainingModel trainingModel, string dayOfWeek, string dayOfMonth)
        {
            if (ModelState.IsValid)
            {
                TraingModelValidator validator = new();
                var v = validator.Validate(trainingModel);
                if (v.IsValid)
                {
                    _context.Add(trainingModel);
                    await _context.SaveChangesAsync();
                    // Ustawienie wartości w ViewBag przed przekierowaniem
                    ViewBag.SelectedDayOfMonth = dayOfMonth;
                    return RedirectToAction("Index", new { dayOfWeek = dayOfWeek, dayOfMonth = dayOfMonth });
                    
                }
                    
            }
            
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", trainingModel.UserId);
            return View(trainingModel);
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateInstant(string dayOfMonth, int month, string year)
        {
            // Sprawdź, czy wartości przekazane z poprzedniego widoku są poprawne
            if (!int.TryParse(dayOfMonth, out int parsedDayOfMonth))
            {
                // Jeśli numer dnia miesiąca nie jest prawidłowy, zwróć błąd
                return BadRequest("Invalid day of month.");
            }

            try
            {
                // Utwórz nowy model treningu
                var trainingModel = new TrainingModel
                {
                    // Ustaw UserId na 1, ponieważ nie masz jeszcze logowania
                    UserId = 1,

                    // Ustaw TrainingDate na datę przekazaną z poprzedniego widoku
                    TrainingDate = new DateTime(int.Parse(year), month, parsedDayOfMonth)
                };

                // Dodaj model treningu do kontekstu bazy danych
                _context.Add(trainingModel);

                // Zapisz zmiany w bazie danych
                await _context.SaveChangesAsync();

                // Przekieruj użytkownika z powrotem do widoku Training z wybranym dniem
                return RedirectToAction("Index", new { dayOfMonth = dayOfMonth, month = month, year = year });
            }
            catch (Exception ex)
            {
                // Obsłuż błąd i zwróć odpowiedź
                return BadRequest($"An error occurred: {ex.Message}");
            }
        }


        // GET: Training/Edit/5
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
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Last_name", trainingModel.UserId);
            return View(trainingModel);
        }

        // POST: Training/Edit/5
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

        // GET: Training/Delete/5
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

        // POST: Training/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int[] trainingIds, string dayOfMonth, int month, string year)
        {
            foreach (var id in trainingIds)
            {
                var trainingModel = await _context.Trainings.FindAsync(id);
                if (trainingModel != null)
                {
                    _context.Trainings.Remove(trainingModel);
                }
            }

            await _context.SaveChangesAsync();
    
            // Przekierowanie z powrotem do akcji Index z odpowiednimi parametrami URL
            return RedirectToAction("Index", new { dayOfMonth = dayOfMonth, month = month, year = year });
        }

        private bool TrainingModelExists(int id)
        {
            return _context.Trainings.Any(e => e.Id == id);
        }
        
        // GET: Training/AddExercise
        public IActionResult AddExercise(string dayOfMonth, int month, string year)
        {
            ViewBag.SelectedDayOfMonth = dayOfMonth;
            ViewBag.DisplayedMonth = month;
            ViewBag.DisplayedYear = year;

            return View();
        }

        // POST: Training/AddExercise
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddExercise(string dayOfMonth, int month, string year, int trainingId, [Bind("Id,ExerciseName")] ExerciseModel exerciseModel)        
        {
            if (ModelState.IsValid)
            {
                // Znajdź trening na podstawie daty
                DateTime selectedDate = new DateTime(int.Parse(year), month, int.Parse(dayOfMonth));
                var training = await _context.Trainings.FirstOrDefaultAsync(t => t.TrainingDate.Date == selectedDate.Date);

                if (training == null)
                {
                    // Jeśli trening nie istnieje, utwórz nowy
                    training = new TrainingModel
                    {
                        UserId = 1, // Tutaj należy wprowadzić właściwego użytkownika
                        TrainingDate = selectedDate
                    };
                    _context.Add(training);
                    await _context.SaveChangesAsync();
                }

                // Dodaj ćwiczenie do treningu
                exerciseModel.TrainingId = training.Id;
                _context.Add(exerciseModel);
                await _context.SaveChangesAsync();

                // Przekieruj do akcji Index kontrolera "Training" z odpowiednimi parametrami URL
                return RedirectToAction("Index", "Training", new { dayOfMonth = dayOfMonth, month = month, year = year });
            }

            // W przypadku nieprawidłowych danych, zwróć widok "AddExercise" z błędami
            ViewBag.SelectedDayOfMonth = dayOfMonth;
            ViewBag.DisplayedMonth = month;
            ViewBag.DisplayedYear = year;
            return RedirectToAction("Index", new { dayOfMonth = dayOfMonth, month = month, year = year });
        }
        
        // POST: Training/AddSeries
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSeries(string dayOfMonth, int month, string year,int trainingId, int exerciseId, int seriesAmount, int repetitions, int weight)
        {
            
                // Pobierz trening na podstawie jego identyfikatora
                var training = await _context.Trainings
                    .Include(t => t.Exercises)
                    .FirstOrDefaultAsync(t => t.Id == trainingId);


                // Sprawdź, czy trening istnieje
                if (training == null)
                {
                    return NotFound("Training not found.");
                }

                // Znajdź ćwiczenie w ramach treningu na podstawie jego identyfikatora
                var exercise = training.Exercises.FirstOrDefault(e => e.Id == exerciseId);
                if (exercise == null)
                {
                    return NotFound("Exercise not found in the training.");
                }

                // Utwórz nową serię ćwiczeń
                var exerciseSeries = new ExerciseSeriesModel
                {
                    ExerciseId = exerciseId,
                    SeriesAmount = seriesAmount,
                    Repetitions = repetitions,
                    Weight = weight
                };

                // Dodaj serię ćwiczeń do kontekstu bazy danych
                _context.ExercisesSeries.Add(exerciseSeries);

                // Zapisz zmiany w bazie danych
                await _context.SaveChangesAsync();

                // Dodaj serię ćwiczeń do listy serii ćwiczeń dla danego ćwiczenia w ramach treningu
                exercise.ExerciseSeries.Add(exerciseSeries);

                // Zaktualizuj trening w bazie danych
                _context.Trainings.Update(training);
                await _context.SaveChangesAsync();

                // Przekieruj użytkownika z powrotem do widoku szczegółów treningu
                return RedirectToAction("Index", new { dayOfMonth = dayOfMonth, month = month, year = year });
 
        }
        
        [HttpPost]
        public async Task<IActionResult> UpdateSeries(int dayOfMonth, int month, int year, int seriesId, int repetitions, int weight)
        {
            // Znajdź odpowiednią serię na podstawie seriesId
            var series = await _context.ExercisesSeries.FindAsync(seriesId);

            if (series == null)
            {
                return NotFound();
            }

            // Aktualizuj dane serii
            series.Repetitions = repetitions;
            series.Weight = weight;

            // Zapisz zmiany w bazie danych
            await _context.SaveChangesAsync();

            // Zwróć status 200 OK, aby poinformować, że operacja się powiodła
            return Ok();
        }
        
        



    }
}