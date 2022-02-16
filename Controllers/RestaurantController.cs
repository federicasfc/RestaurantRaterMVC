using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantRaterMVC.Data;
using RestaurantRaterMVC.Models.Restaurant;

namespace RestaurantRaterMVC.Controllers
{
    public class RestaurantController : Controller
    {
        private RestaurantDbContext _context;

        public RestaurantController(RestaurantDbContext context)
        {
            _context = context;
        }

        //Equivalent of GetAll
        //Default GetMethod, could use an [httpGet] annotation if we wanted, but unnecessary

        public async Task<IActionResult> Index()
        {
            List<RestaurantListItem> restaurants = await _context.Restaurants
            .Include(r => r.Ratings)
            .Select(r => new RestaurantListItem()
            {
                Id = r.Id,
                Name = r.Name,
                Score = r.Score
            }).ToListAsync();

            return View(restaurants);
        }

        //Get method
        //Will return the Html form view that we'll use to create restaurants
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //PostMethod for Create
        //will take in the form data from the create view that EntityFramework will deserialize into the right format (C# RestaurantCreate model object)

        public async Task<IActionResult> Create(RestaurantCreate model)
        {
            //if model invalid, return Create view with existing model and an error message added to with ViewBag, which can pass extra info to a View
            if (!ModelState.IsValid)
            {
                ViewBag.errorMessage = ModelState.FirstOrDefault().Value.Errors.FirstOrDefault().ErrorMessage;
                return View(model);

            }
            //if valid make new Restaurant(entity)
            Restaurant restaurant = new Restaurant()
            {
                Name = model.Name,
                Location = model.Location
            };

            //add to dbcontext and save to db
            _context.Restaurants.Add(restaurant);
            await _context.SaveChangesAsync();

            //go back to Restaurant index and see new restaurant on list

            return RedirectToAction(nameof(Index)); //When we do n-tier, will we go back to using a bool verification, or does something here render it unnecessary
        }
    }
}