using System;
using System.Collections.Generic;

namespace eproject3.Models.Generated;

public partial class Movie
{
    public int MovieId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? TrailerUrl { get; set; }

    public int? Duration { get; set; }

    public DateOnly? ReleaseDate { get; set; }

    public bool? IsPopular { get; set; }

    public bool? IsHot { get; set; }

    public string? CoverImage { get; set; }

    public virtual ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
    
    public virtual ICollection<MovieShowtime> MovieShowtimes { get; set; } = new List<MovieShowtime>();
}
