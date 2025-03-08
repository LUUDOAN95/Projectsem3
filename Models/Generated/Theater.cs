using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Theater
{
    public int TheaterId { get; set; }

    public string TheaterName { get; set; } = null!;

    public int SeatCapacity { get; set; }

    public virtual ICollection<Seat> Seats { get; set; } = new List<Seat>();

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
