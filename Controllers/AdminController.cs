using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using eproject3.Models.Generated;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace eproject3.Controllers
{
    [Authorize(AuthenticationSchemes = "AdminAuth")]
    public class AdminController : Controller
    {
        private readonly ABCDMallContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(ABCDMallContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Dashboard");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password)
        {
            // Validate credentials (use secure methods in production)
            if (username == "admin" && password == "admin123")
            {
                var claims = new[] { 
                    new Claim(ClaimTypes.Name, "admin"),
                    new Claim(ClaimTypes.Role, "Admin")  // Add role claim
                };
                var identity = new ClaimsIdentity(claims, "AdminAuth");
                await HttpContext.SignInAsync("AdminAuth", new ClaimsPrincipal(identity));
                return RedirectToAction("Dashboard");
            }
            ViewData["ErrorMessage"] = "Invalid username or password";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync("AdminAuth");
            return RedirectToAction("Login");
        }

        public IActionResult Dashboard()
        {
            // Add statistics to ViewBag
            ViewBag.ShopCount = _context.Shops.Count(s => s.IsDeleted != true);
            ViewBag.MovieCount = _context.Movies.Count();
            ViewBag.ShowtimeCount = _context.MovieShowtimes.Count();
            ViewBag.BookingCount = _context.Bookings.Count();
            
            return View();
        }

        // Shop Management
        public IActionResult ManageShops()
        {
            return View(_context.Shops.ToList());
        }

        [HttpPost]
        public IActionResult AddShop(string shopName, string category, string description, IFormFile imageFile)
        {
            try
            {
                string imageUrl = ProcessUploadedImage(imageFile, "shops");
                
                var shop = new Shop
                {
                    ShopName = shopName,
                    Category = category,
                    Description = description,
                    ImageUrl = imageUrl,
                    IsDeleted = false
                };
                
                _context.Shops.Add(shop);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Shop added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding shop: " + ex.Message;
            }
            
            return RedirectToAction("ManageShops");
        }

        [HttpPost]
        public IActionResult EditShop(int shopId, string shopName, string category, string description, IFormFile imageFile)
        {
            try
            {
                var shop = _context.Shops.Find(shopId);
                if (shop == null)
                {
                    TempData["ErrorMessage"] = "Shop not found";
                    return RedirectToAction("ManageShops");
                }
                
                shop.ShopName = shopName;
                shop.Category = category;
                shop.Description = description;
                
                if (imageFile != null)
                {
                    // Upload new image and update the URL
                    shop.ImageUrl = ProcessUploadedImage(imageFile, "shops");
                }
                
                _context.Update(shop);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Shop updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating shop: " + ex.Message;
            }
            
            return RedirectToAction("ManageShops");
        }

        [HttpPost]
        public IActionResult DeleteShop(int id)
        {
            try
            {
                var shop = _context.Shops.Find(id);
                if (shop != null)
                {
                    shop.IsDeleted = true;
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Shop deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Shop not found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting shop: " + ex.Message;
            }
            
            return RedirectToAction("ManageShops");
        }

        // Movie Management
        public IActionResult ManageMovies()
        {
            return View(_context.Movies.ToList());
        }
        
        [HttpPost]
        public IActionResult AddMovie(string title, string description, int duration, 
            DateTime releaseDate, string trailerUrl, IFormFile coverFile, bool isHot = false, bool isPopular = false)
        {
            try
            {
                string coverImageUrl = ProcessUploadedImage(coverFile, "movies");
                
                var movie = new Movie
                {
                    Title = title,
                    Description = description,
                    Duration = duration,
                    ReleaseDate = DateOnly.FromDateTime(releaseDate),
                    TrailerUrl = trailerUrl,
                    CoverImage = coverImageUrl,
                    IsHot = isHot,
                    IsPopular = isPopular
                };
                
                _context.Movies.Add(movie);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Movie added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding movie: " + ex.Message;
            }
            
            return RedirectToAction("ManageMovies");
        }
        
        [HttpPost]
        public IActionResult EditMovie(int movieId, string title, string description, int duration, 
            DateTime? releaseDate, string trailerUrl, IFormFile coverFile, bool isHot = false, bool isPopular = false)
        {
            try
            {
                var movie = _context.Movies.Find(movieId);
                if (movie == null)
                {
                    TempData["ErrorMessage"] = "Movie not found";
                    return RedirectToAction("ManageMovies");
                }
                
                movie.Title = title;
                movie.Description = description;
                movie.Duration = duration;
                if (releaseDate.HasValue)
                {
                    movie.ReleaseDate = DateOnly.FromDateTime(releaseDate.Value);
                }
                movie.TrailerUrl = trailerUrl;
                movie.IsHot = isHot;
                movie.IsPopular = isPopular;
                
                if (coverFile != null)
                {
                    // Upload new image and update the URL
                    movie.CoverImage = ProcessUploadedImage(coverFile, "movies");
                }
                
                _context.Update(movie);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Movie updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating movie: " + ex.Message;
            }
            
            return RedirectToAction("ManageMovies");
        }
        
        [HttpPost]
        public IActionResult DeleteMovie(int id)
        {
            try
            {
                var movie = _context.Movies.Find(id);
                if (movie != null)
                {
                    _context.Movies.Remove(movie);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Movie deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Movie not found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting movie: " + ex.Message;
            }
            
            return RedirectToAction("ManageMovies");
        }

        // Ticket Management
        [Authorize]
        public IActionResult ManageTickets()
        {
            ViewBag.Movies = _context.Movies.ToList();

            // Check if the table exists or create a mapping
            var showtimes = new List<MovieShowtime>();
            try 
            {
                // Try to get data from MovieShowtimes table
                showtimes = _context.MovieShowtimes
                    .Include(s => s.Movie)
                    .ToList();
            }
            catch (Exception ex)
            {
                // If there's an issue, log it and return an empty list
                Console.WriteLine($"Error accessing MovieShowtimes: {ex.Message}");
                // We'll provide an empty list so the view still renders
            }
            
            return View(showtimes);
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult AddShowtime(int movieId, DateTime showDate, TimeSpan startTime, 
            int hallNumber, decimal ticketPrice, int availableSeats)
        {
            try
            {
                var showtime = new MovieShowtime
                {
                    MovieId = movieId,
                    ShowDate = DateOnly.FromDateTime(showDate),
                    StartTime = startTime,
                    HallNumber = hallNumber,
                    TicketPrice = ticketPrice,
                    AvailableSeats = availableSeats
                };
                
                _context.MovieShowtimes.Add(showtime);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Showtime added successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error adding showtime: " + ex.Message;
            }
            
            return RedirectToAction("ManageTickets");
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult EditShowtime(int showtimeId, int movieId, DateTime showDate, 
            TimeSpan startTime, int hallNumber, decimal ticketPrice, int availableSeats)
        {
            try
            {
                var showtime = _context.MovieShowtimes.Find(showtimeId);
                if (showtime == null)
                {
                    TempData["ErrorMessage"] = "Showtime not found";
                    return RedirectToAction("ManageTickets");
                }
                
                showtime.MovieId = movieId;
                showtime.ShowDate = DateOnly.FromDateTime(showDate);
                showtime.StartTime = startTime;
                showtime.HallNumber = hallNumber;
                showtime.TicketPrice = ticketPrice;
                showtime.AvailableSeats = availableSeats;
                
                _context.Update(showtime);
                _context.SaveChanges();
                
                TempData["SuccessMessage"] = "Showtime updated successfully!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error updating showtime: " + ex.Message;
            }
            
            return RedirectToAction("ManageTickets");
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult DeleteShowtime(int id)
        {
            try
            {
                var showtime = _context.MovieShowtimes.Find(id);
                if (showtime != null)
                {
                    _context.MovieShowtimes.Remove(showtime);
                    _context.SaveChanges();
                    TempData["SuccessMessage"] = "Showtime deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Showtime not found";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Error deleting showtime: " + ex.Message;
            }
            
            return RedirectToAction("ManageTickets");
        }
        
        [HttpGet]
        [Authorize]
        public IActionResult GetBookings(int showtimeId)
        {
            try
            {
                var bookings = _context.Bookings
                    .Where(b => b.ShowtimeId == showtimeId)
                    .ToList();
                
                return Json(bookings);
            }
            catch (Exception)
            {
                return Json(new { error = "Error fetching booking data" });
            }
        }

        [HttpGet]
        public ActionResult GetShowtimesByMovie(int movieId)
        {
            // Check if it's an AJAX request
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                // For non-AJAX requests, redirect to the manage tickets page
                return RedirectToAction("ManageMovies");
            }

            try
            {
                var showtimes = _context.MovieShowtimes
                    .Where(s => s.MovieId == movieId)
                    .OrderBy(s => s.ShowDate)
                    .ThenBy(s => s.StartTime)
                    .ToList();
                    
                return Json(showtimes);
            }
            catch (Exception ex)
            {
                // Always return JSON, even for errors
                return Json(new List<object>());
            }
        }

        [HttpPost]
        public ActionResult AddShowtimeJson([FromBody] MovieShowtime model)
        {
            // Check if it's an AJAX request
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                // For non-AJAX requests, return a JSON error
                return Json(new { success = false, message = "Invalid request format." });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = $"Validation failed: {errors}" });
                }

                Console.WriteLine($"AddShowtimeJson - Received: {DebugMovieShowtime(model)}");

                // Create a new showtime with explicit property assignments
                var showtime = new MovieShowtime
                {
                    MovieId = model.MovieId,
                    ShowDate = model.ShowDate, 
                    StartTime = TimeSpan.Parse(model.StartTime.ToString()),
                    HallNumber = model.HallNumber,
                    TicketPrice = model.TicketPrice,
                    AvailableSeats = model.AvailableSeats
                };
                
                Console.WriteLine($"AddShowtimeJson - Created: {DebugMovieShowtime(showtime)}");
                
                // Add to context and save changes
                _context.MovieShowtimes.Add(showtime);
                var result = _context.SaveChanges();
                
                Console.WriteLine($"AddShowtimeJson - SaveChanges result: {result}");
                
                if (result > 0)
                {
                    return Json(new { success = true, message = "Showtime added successfully!", data = showtime });
                }
                else
                {
                    return Json(new { success = false, message = "No changes were saved to the database." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"AddShowtimeJson - Exception: {ex}");
                return Json(new { success = false, message = $"Error adding showtime: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public ActionResult EditShowtimeJson([FromBody] MovieShowtime model)
        {
            // Check if it's an AJAX request
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                // For non-AJAX requests, return a JSON error
                return Json(new { success = false, message = "Invalid request format." });
            }

            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));
                    return Json(new { success = false, message = $"Validation failed: {errors}" });
                }
                
                Console.WriteLine($"EditShowtimeJson - Received: {DebugMovieShowtime(model)}");
                
                var existingShowtime = _context.MovieShowtimes.Find(model.ShowtimeId);
                if (existingShowtime == null)
                {
                    return Json(new { success = false, message = "Showtime not found" });
                }
                
                Console.WriteLine($"EditShowtimeJson - Before update: {DebugMovieShowtime(existingShowtime)}");
                
                // Explicitly update each property
                existingShowtime.MovieId = model.MovieId;
                existingShowtime.ShowDate = model.ShowDate;
                existingShowtime.StartTime = TimeSpan.Parse(model.StartTime.ToString());
                existingShowtime.HallNumber = model.HallNumber;
                existingShowtime.TicketPrice = model.TicketPrice;
                existingShowtime.AvailableSeats = model.AvailableSeats;
                
                Console.WriteLine($"EditShowtimeJson - After update: {DebugMovieShowtime(existingShowtime)}");
                
                // Mark as modified and save changes
                _context.Entry(existingShowtime).State = EntityState.Modified;
                var result = _context.SaveChanges();
                
                Console.WriteLine($"EditShowtimeJson - SaveChanges result: {result}");
                
                if (result > 0)
                {
                    return Json(new { success = true, message = "Showtime updated successfully!", data = existingShowtime });
                }
                else
                {
                    return Json(new { success = false, message = "No changes were saved to the database." });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EditShowtimeJson - Exception: {ex}");
                return Json(new { success = false, message = $"Error updating showtime: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }

        [HttpPost]
        public ActionResult DeleteShowtimeJson(int id)
        {
            // Check if it's an AJAX request
            if (!Request.Headers["X-Requested-With"].Equals("XMLHttpRequest"))
            {
                // For non-AJAX requests, return a JSON error
                return Json(new { success = false, message = "Invalid request format." });
            }

            try
            {
                var showtime = _context.MovieShowtimes.Find(id);
                if (showtime != null)
                {
                    _context.MovieShowtimes.Remove(showtime);
                    var result = _context.SaveChanges();
                    
                    if (result > 0)
                    {
                        return Json(new { success = true, message = "Showtime deleted successfully!" });
                    }
                    else
                    {
                        return Json(new { success = false, message = "No changes were saved to the database." });
                    }
                }
                else
                {
                    return Json(new { success = false, message = "Showtime not found" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteShowtimeJson - Exception: {ex}");
                return Json(new { success = false, message = $"Error deleting showtime: {ex.Message}", stackTrace = ex.StackTrace });
            }
        }

        // Helper method for debugging model binding issues
        private string DebugMovieShowtime(MovieShowtime model)
        {
            return $"MovieId: {model.MovieId}, " +
                   $"ShowDate: {model.ShowDate}, " +
                   $"StartTime: {model.StartTime}, " +
                   $"HallNumber: {model.HallNumber}, " +
                   $"TicketPrice: {model.TicketPrice}, " +
                   $"AvailableSeats: {model.AvailableSeats}";
        }

        // Helper Methods
        private string ProcessUploadedImage(IFormFile imageFile, string folder)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                return $"/images/default-{folder.TrimEnd('s')}.jpg"; // Default image path
            }

            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images", folder);
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                imageFile.CopyTo(fileStream);
            }
            
            return $"/images/{folder}/{uniqueFileName}";
        }
    }
}