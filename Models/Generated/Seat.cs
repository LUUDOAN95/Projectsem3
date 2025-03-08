using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Seat
{
    public int SeatId { get; set; }

    public int? TheaterId { get; set; }

    public string SeatCode { get; set; } = null!;

    public bool? IsBooked { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Theater? Theater { get; set; }
}
