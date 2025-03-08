using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eproject3.Models.Generated;

[Table("Bookings")]
public partial class Booking
{
    [Key]
    public Guid BookingId { get; set; }

    [ForeignKey(nameof(MovieShowtime))]
    public int? ShowtimeId { get; set; }

    [ForeignKey(nameof(Seat))]
    public int? SeatId { get; set; }

    [StringLength(100)]
    public string? CustomerName { get; set; }

    [StringLength(100)]
    public string? Email { get; set; }

    [StringLength(20)]
    public string? Phone { get; set; }

    public DateTime? BookingTime { get; set; }

    public DateTime? ExpiryTime { get; set; }

    [StringLength(20)]
    public string? PaymentStatus { get; set; }

    [Column("QRCode")]
    [StringLength(100)]
    public string? Qrcode { get; set; }

    public virtual Seat? Seat { get; set; }

    public virtual MovieShowtime? MovieShowtime { get; set; }
}
