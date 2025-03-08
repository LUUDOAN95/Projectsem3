using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Hangfire;
using QRCoder;
using eproject3.Models.Generated;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace eproject3.Controllers
{
    public class BookingController : Controller
    {
        private readonly ABCDMallContext _context;
        private readonly ILogger<BookingController> _logger;

        public BookingController(ABCDMallContext context, ILogger<BookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(BookingModel model)
        {
            try
            {
                _logger.LogInformation("Processing payment for showtime {ShowtimeId} and seat {SeatId}", model.ShowtimeId, model.SeatId);
                
                // Verify showtime exists
                var showtime = await _context.MovieShowtimes
                    .Include(s => s.Movie)
                    .FirstOrDefaultAsync(s => s.ShowtimeId == model.ShowtimeId);
                
                if (showtime == null)
                {
                    _logger.LogWarning("Showtime {ShowtimeId} not found", model.ShowtimeId);
                    TempData["ErrorMessage"] = "Selected showtime not found.";
                    return RedirectToAction("Index", "Movie");
                }

                // Create booking
                var bookingId = Guid.NewGuid();
                var booking = new Booking
                {
                    BookingId = bookingId,
                    ShowtimeId = model.ShowtimeId,
                    SeatId = model.SeatId,
                    CustomerName = model.CustomerName,
                    Email = model.Email,
                    Phone = model.Phone,
                    BookingTime = DateTime.UtcNow,
                    ExpiryTime = DateTime.UtcNow.AddMinutes(15),
                    PaymentStatus = "Pending",
                    Qrcode = GenerateQRCode()
                };

                _context.Bookings.Add(booking);
                await _context.SaveChangesAsync();

                _logger.LogInformation("Booking created with ID {BookingId}", bookingId);

                // Store booking ID in TempData for payment processing
                TempData["BookingId"] = bookingId;

                try
                {
                    // Start payment timeout
                    BackgroundJob.Schedule(() => ReleaseUnpaidSeats(bookingId), 
                        TimeSpan.FromMinutes(15));
                    
                    _logger.LogInformation("Scheduled job to release unpaid seats for booking {BookingId}", bookingId);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to schedule background job for booking {BookingId}", bookingId);
                    // Continue even if Hangfire fails
                }

                // Redirect directly to payment page
                _logger.LogInformation("Redirecting to payment page for booking {BookingId}", bookingId);
                return RedirectToAction("Process", "Payment", new { id = bookingId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing payment");
                TempData["ErrorMessage"] = $"An error occurred while processing your booking: {ex.Message}";
                return RedirectToAction("Index", "Movie");
            }
        }

        private string GenerateQRCode()
        {
            using QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(Guid.NewGuid().ToString(), QRCodeGenerator.ECCLevel.Q);
            PngByteQRCode qrCode = new PngByteQRCode(qrCodeData);
            return Convert.ToBase64String(qrCode.GetGraphic(20));
        }

        public async Task<IActionResult> ReleaseUnpaidSeats(Guid bookingId)
        {
            try
            {
                _logger.LogInformation("Releasing unpaid seats for booking {BookingId}", bookingId);
                var booking = await _context.Bookings.FindAsync(bookingId);
                if (booking != null && booking.PaymentStatus == "Pending")
                {
                    _context.Bookings.Remove(booking);
                    await _context.SaveChangesAsync();
                    _logger.LogInformation("Released unpaid seats for booking {BookingId}", bookingId);
                }
                else
                {
                    _logger.LogInformation("Booking {BookingId} not found or already paid", bookingId);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error releasing unpaid seats for booking {BookingId}", bookingId);
                return StatusCode(500);
            }
        }
    }

    // This class should be moved to a separate file
    public class BookingModel
    {
        public int ShowtimeId { get; set; }
        public int SeatId { get; set; }
        public required string CustomerName { get; set; }
        public required string Email { get; set; }
        public required string Phone { get; set; }
    }
}