using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Showtime
{
    public int ShowtimeId { get; set; }

    public int? MovieId { get; set; }

    public int? TheaterId { get; set; }

    public DateTime StartTime { get; set; }

    public DateTime EndTime { get; set; }

    public virtual ICollection<Booking> Bookings { get; set; } = new List<Booking>();

    public virtual Movie? Movie { get; set; }

    public virtual Theater? Theater { get; set; }
}
