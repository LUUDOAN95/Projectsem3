using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eproject3.Models.Generated;

[Table("MovieShowtimes")]
public partial class MovieShowtime
{
    public MovieShowtime()
    {
        Bookings = new HashSet<Booking>();
    }

    [Key]
    public int ShowtimeId { get; set; }

    [Required]
    public int MovieId { get; set; }

    [Required]
    public DateOnly ShowDate { get; set; }

    [Required]
    public TimeSpan StartTime { get; set; }

    [Required]
    public int HallNumber { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal TicketPrice { get; set; }

    [Required]
    public int AvailableSeats { get; set; }

    [ForeignKey("MovieId")]
    public virtual Movie? Movie { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; }
} 