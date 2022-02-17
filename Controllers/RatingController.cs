using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Rating;

namespace RestaurantRaterMVC.Controllers
{

    public class RatingController : Controller
    {
        private RestaurantDbContext _context;
        public RatingController(RestaurantDbContext context)
        {
            _context = context;
        }

        //GetAllRatings/ Index Method
        //more specifically get all ratings of all restaurants? not actually sure what this is returning

        public async Task<IActionResult> Index()
        {
            IEnumerable<RatingListItem> ratings = await _context.Ratings
            .Select(r => new RatingListItem()
            {
                RestaurantName = r.Restaurant.Name,
                Score = r.Score
            }).ToListAsync();

            return View(ratings);
        } //In the event that the modules don't make a link to this method, do so at bottom of Restaurant Detail view

        //GetAllRatings connected to a specific restaurant

        public async Task<IActionResult> Restaurant(int id)
        {
            IEnumerable<RatingListItem> ratings = await _context.Ratings
            .Where(r => r.RestaurantId == id)
            .Select(r => new RatingListItem()
            {
                RestaurantName = r.Restaurant.Name,
                Score = r.Score
            }).ToListAsync();

            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            ViewBag.RestaurantName = restaurant.Name;
            //Reminder: ViewBag is "dynamic object", so any property can be added to it
            //This will attach the name of the restaurant to the view

            return View(ratings);

        }

    }
}