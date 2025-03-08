// HomeController.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Linq;
using eproject3.Models.Generated;
using System.Collections.Generic;
using eproject3.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace eproject3.Controllers
{
    public class HomeController : Controller
    {
        private readonly ABCDMallContext _context;

        public HomeController(ABCDMallContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            try
            {
                var model = new HomeViewModel
                {
                    FeaturedShops = _context.Shops.Where(s => s.IsDeleted != true).Take(4).ToList() ?? new List<Shop>(),
                    LatestMovies = _context.Movies.OrderByDescending(m => m.ReleaseDate).Take(3).ToList() ?? new List<Movie>()
                };

                return View(model);
            }
            catch (Exception ex)
            {
                // Log the error
                Console.WriteLine($"Error in HomeController.Index: {ex.Message}");
                return View(new HomeViewModel 
                { 
                    FeaturedShops = new List<Shop>(),
                    LatestMovies = new List<Movie>()
                });
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View();
        }
    }
}

// ShopController.cs
namespace eproject3.Controllers
{
    public class ShopController : Controller
    {
        private readonly ABCDMallContext _context;

        public ShopController(ABCDMallContext context)
        {
            _context = context;
        }

        public IActionResult Index(string search)
        {
            var shops = _context.Shops.Where(s => s.IsDeleted == false);
            
            if (!string.IsNullOrEmpty(search))
            {
                shops = shops.Where(s => s.ShopName.Contains(search));
            }
            
            return View(shops.ToList());
        }
    }
}