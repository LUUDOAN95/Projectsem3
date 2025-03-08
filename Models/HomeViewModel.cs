using System.Collections.Generic;
using eproject3.Models.Generated;

namespace eproject3.Models
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            FeaturedShops = new List<Shop>();
            LatestMovies = new List<Movie>();
        }

        public List<Shop> FeaturedShops { get; set; }
        public List<Movie> LatestMovies { get; set; }
    }
} 