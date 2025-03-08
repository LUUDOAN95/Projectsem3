using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using eproject3.Models.Generated;
using Microsoft.EntityFrameworkCore;
using eproject3.Models;
using System.Globalization;
using System.Text;
using Microsoft.Extensions.Logging;

namespace eproject3.Controllers
{
    public class PaymentController : Controller
    {
        private readonly ABCDMallContext _context;
        private readonly IConfiguration _configuration;
        private readonly ILogger<PaymentController> _logger;

        public PaymentController(ABCDMallContext context, IConfiguration configuration, ILogger<PaymentController> logger)
        {
            _context = context;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<IActionResult> Process(Guid? id = null)
        {
            _logger.LogInformation("Processing payment with ID: {BookingId}", id);
            
            Booking booking;
            
            if (id.HasValue)
            {
                // Find the specific booking by ID
                booking = await _context.Bookings
                    .Include(b => b.MovieShowtime)
                    .ThenInclude(s => s!.Movie)
                    .Include(b => b.Seat)
                    .FirstOrDefaultAsync(b => b.BookingId == id);
                    
                if (booking == null)
                {
                    _logger.LogWarning("Booking with ID {BookingId} not found", id);
                    TempData["ErrorMessage"] = $"Booking with ID {id} not found.";
                    return RedirectToAction("Index", "Movie");
                }
                
                _logger.LogInformation("Found booking with ID {BookingId}", id);
            }
            else
            {
                // Try to get booking ID from TempData
                if (TempData.TryGetValue("BookingId", out var tempDataIdObj) && tempDataIdObj is Guid tempDataId)
                {
                    _logger.LogInformation("Using booking ID from TempData: {BookingId}", tempDataId);
                    
                    booking = await _context.Bookings
                        .Include(b => b.MovieShowtime)
                        .ThenInclude(s => s!.Movie)
                        .Include(b => b.Seat)
                        .FirstOrDefaultAsync(b => b.BookingId == tempDataId);
                        
                    if (booking == null)
                    {
                        _logger.LogWarning("Booking with ID {BookingId} from TempData not found", tempDataId);
                        TempData["ErrorMessage"] = $"Booking with ID {tempDataId} not found.";
                        return RedirectToAction("Index", "Movie");
                    }
                }
                else
                {
                    _logger.LogInformation("No booking ID provided, looking for most recent pending booking");
                    
                    // Find the most recent pending booking as fallback
                    booking = await _context.Bookings
                        .Include(b => b.MovieShowtime)
                        .ThenInclude(s => s!.Movie)
                        .Include(b => b.Seat)
                        .OrderByDescending(b => b.BookingTime)
                        .FirstOrDefaultAsync(b => b.PaymentStatus == "Pending");
                        
                    if (booking == null)
                    {
                        _logger.LogWarning("No pending booking found");
                        TempData["ErrorMessage"] = "No pending booking found.";
                        return RedirectToAction("Index", "Movie");
                    }
                    
                    _logger.LogInformation("Found most recent pending booking with ID {BookingId}", booking.BookingId);
                }
            }

            if (booking.MovieShowtime == null)
            {
                _logger.LogWarning("Invalid booking: No showtime found for booking {BookingId}", booking.BookingId);
                TempData["ErrorMessage"] = "Invalid booking: No showtime found.";
                return RedirectToAction("Index", "Movie");
            }

            var model = new PaymentViewModel
            {
                BookingId = booking.BookingId,
                MovieTitle = booking.MovieShowtime.Movie?.Title ?? "Unknown Movie",
                ShowtimeDate = booking.MovieShowtime.ShowDate,
                ShowtimeTime = booking.MovieShowtime.StartTime,
                SeatCode = booking.Seat?.SeatCode ?? "Unknown Seat",
                Amount = booking.MovieShowtime.TicketPrice,
                CustomerName = booking.CustomerName ?? "",
                Email = booking.Email ?? "",
                Phone = booking.Phone ?? ""
            };

            _logger.LogInformation("Rendering payment view for booking {BookingId}", booking.BookingId);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ProcessPayment(PaymentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Process", model);
            }

            var booking = await _context.Bookings
                .Include(b => b.MovieShowtime)
                .ThenInclude(s => s!.Movie)
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == model.BookingId);

            if (booking == null)
            {
                TempData["ErrorMessage"] = "Booking not found.";
                return RedirectToAction("Index", "Home");
            }

            if (booking.MovieShowtime == null)
            {
                TempData["ErrorMessage"] = "Invalid booking: No movie showtime found.";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                // Create VNPAY payment URL
                var vnpayUrl = CreateVNPayUrl(booking, booking.MovieShowtime);
                return Redirect(vnpayUrl);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Payment processing failed: {ex.Message}");
                return View("Process", model);
            }
        }

        private string CreateVNPayUrl(Booking booking, MovieShowtime movieShowtime)
        {
            var timeZoneById = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var timeNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, timeZoneById);
            var tick = DateTime.Now.Ticks.ToString();
            var pay = new VNPayLibrary();

            var urlCallBack = $"{Request.Scheme}://{Request.Host}/Payment/VNPayReturn";

            pay.AddRequestData("vnp_Version", VNPayConfig.Version);
            pay.AddRequestData("vnp_Command", VNPayConfig.Command);
            pay.AddRequestData("vnp_TmnCode", VNPayConfig.TmnCode);
            pay.AddRequestData("vnp_Amount", ((int)(movieShowtime.TicketPrice * 100) * 100).ToString());
            pay.AddRequestData("vnp_CreateDate", timeNow.ToString("yyyyMMddHHmmss"));
            pay.AddRequestData("vnp_CurrCode", VNPayConfig.CurrCode);
            pay.AddRequestData("vnp_IpAddr", VNPayConfig.GetIpAddress());
            pay.AddRequestData("vnp_Locale", VNPayConfig.Locale);
            pay.AddRequestData("vnp_OrderInfo", $"Payment for Movie Ticket - {movieShowtime.Movie?.Title ?? "Unknown Movie"} - Seat {booking.Seat?.SeatCode ?? "Unknown"}");
            pay.AddRequestData("vnp_OrderType", VNPayConfig.OrderType);
            pay.AddRequestData("vnp_ReturnUrl", urlCallBack);
            pay.AddRequestData("vnp_TxnRef", booking.BookingId.ToString());

            var paymentUrl = pay.CreateRequestUrl(VNPayConfig.PaymentUrl, VNPayConfig.HashSecret);

            return paymentUrl;
        }

        public async Task<IActionResult> VNPayReturn([FromQuery] Dictionary<string, string> vnpayData)
        {
            var pay = new VNPayLibrary();
            foreach (var (key, value) in vnpayData)
            {
                pay.AddResponseData(key, value);
            }

            var orderId = Guid.Parse(pay.GetResponseData("vnp_TxnRef"));
            var vnPayTranId = Convert.ToInt64(pay.GetResponseData("vnp_TransactionNo"));
            var vnpResponseCode = pay.GetResponseData("vnp_ResponseCode");
            var vnpSecureHash = pay.GetResponseData("vnp_SecureHash");
            var secretKey = VNPayConfig.HashSecret;
            var checkSignature = pay.ValidateSignature(vnpSecureHash, secretKey);

            if (!checkSignature)
            {
                return RedirectToAction("PaymentError", new { message = "Invalid signature" });
            }

            var booking = await _context.Bookings
                .Include(b => b.MovieShowtime)
                .ThenInclude(s => s!.Movie)
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == orderId);

            if (booking == null)
            {
                return RedirectToAction("PaymentError", new { message = "Booking not found" });
            }

            if (vnpResponseCode == "00")
            {
                // Payment successful
                booking.PaymentStatus = "Completed";
                await _context.SaveChangesAsync();
                return RedirectToAction("Confirmation", new { id = booking.BookingId });
            }
            else
            {
                // Payment failed
                return RedirectToAction("PaymentError", new { message = "Payment failed" });
            }
        }

        public IActionResult PaymentError(string message)
        {
            ViewBag.ErrorMessage = message;
            return View();
        }

        public async Task<IActionResult> Confirmation(Guid id)
        {
            var booking = await _context.Bookings
                .Include(b => b.MovieShowtime)
                .ThenInclude(s => s!.Movie)
                .Include(b => b.Seat)
                .FirstOrDefaultAsync(b => b.BookingId == id);

            if (booking == null)
            {
                return NotFound();
            }

            return View(booking);
        }
    }

    public class PaymentViewModel
    {
        public Guid BookingId { get; set; }
        public string MovieTitle { get; set; } = "";
        public DateOnly ShowtimeDate { get; set; }
        public TimeSpan ShowtimeTime { get; set; }
        public string SeatCode { get; set; } = "";
        public decimal Amount { get; set; }
        public string CustomerName { get; set; } = "";
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
    }
} 