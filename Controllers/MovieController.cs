using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using eproject3.Models.Generated;
using System.Collections.Generic;

namespace eproject3.Controllers
{
    public class MovieController : Controller
    {
        private readonly ABCDMallContext _context;

        public MovieController(ABCDMallContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var movies = _context.Movies
                .Include(m => m.MovieShowtimes)
                .ToList();
            
            var model = new MovieViewModel
            {
                PopularMovies = movies.Where(m => m.IsPopular == true).ToList(),
                HotMovies = movies.Where(m => m.IsHot == true).ToList(),
                AllMovies = movies.ToList()
            };
            return View(model);
        }

        public IActionResult Detail(int id)
        {
            try
            {
                var today = DateOnly.FromDateTime(DateTime.Today);
                var movie = _context.Movies
                    .Include(m => m.MovieShowtimes)
                    .FirstOrDefault(m => m.MovieId == id);
                
                if (movie == null)
                {
                    TempData["ErrorMessage"] = "Movie not found";
                    return RedirectToAction("Index");
                }
                
                // Filter and sort showtimes
                if (movie.MovieShowtimes != null)
                {
                    movie.MovieShowtimes = movie.MovieShowtimes
                        .Where(s => s.ShowDate >= today)
                        .OrderBy(s => s.ShowDate)
                        .ThenBy(s => s.StartTime)
                        .ToList();

                    // Ensure proper TimeSpan formatting for each showtime
                    foreach (var showtime in movie.MovieShowtimes)
                    {
                        if (showtime.StartTime == default)
                        {
                            showtime.StartTime = new TimeSpan(0, 0, 0); // Set default time if none exists
                        }
                    }
                }
                
                return View(movie);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Error loading movie details: {ex.Message}";
                return RedirectToAction("Index");
            }
        }
    }

    // MovieViewModel class
    public class MovieViewModel
    {
        public List<Movie> PopularMovies { get; set; } = new List<Movie>();
        public List<Movie> HotMovies { get; set; } = new List<Movie>();
        public List<Movie> AllMovies { get; set; } = new List<Movie>();
    }
}