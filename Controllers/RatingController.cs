using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
                Id = r.Id,
                RestaurantName = r.Restaurant.Name,
                Score = r.Score,
                RestaurantId = r.RestaurantId

            }).ToListAsync();

            Restaurant restaurant = await _context.Restaurants.FindAsync(id);
            ViewBag.RestaurantName = restaurant.Name;
            //Reminder: ViewBag is "dynamic object", so any property can be added to it
            //This will attach the name of the restaurant to the view

            return View(ratings);

        }

        //CREATE

        //Get Method for Create

        public async Task<IActionResult> Create()
        {
            IEnumerable<SelectListItem> restaurantOptions = await _context.Restaurants
            .Select(r => new SelectListItem()
            {

                Text = r.Name, //text is what gets displayed
                Value = r.Id.ToString() //value is a string, so have to convert; also Id is value here because id is fk between tables

            }).ToListAsync();

            RatingCreate model = new RatingCreate();
            model.RestaurantOptions = restaurantOptions;

            return View(model);  //mod 9.03 for reference
        }


        //Post Method for Create

        [HttpPost]

        public async Task<IActionResult> Create(RatingCreate model)
        {
            if (!ModelState.IsValid)
                return View();

            Rating rating = new Rating()
            {
                RestaurantId = model.RestaurantId,
                Score = model.Score
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
            //return RedirectToAction("Restaurant", new {id = model.RestaurantId}); -- test this
        }


        //DELETE ATTEMPT

        //Get for Delete
        //This goes from backend to browser(user)
        public async Task<IActionResult> Delete(int id)
        {
            //Here: pulling rating by id to be able to package it up into model to show user what they're trying to delete
            Rating rating = await _context.Ratings.Include(r => r.Restaurant).FirstOrDefaultAsync(r => r.Id == id);

            RatingListItem ratingListItem = new RatingListItem()
            {
                Id = rating.Id,
                RestaurantName = rating.Restaurant.Name,
                Score = rating.Score,
                RestaurantId = rating.RestaurantId
            };


            return View(ratingListItem);

        }

        //Post for Delete

        [HttpPost]

        public async Task<IActionResult> Delete(RatingListItem model) //remember deleted int id from parameter here and in RestaurantController
        {
            //Here: pulling rating by id again to actually direct the database to the rating we want deleted.
            Rating rating = await _context.Ratings.FirstOrDefaultAsync(r => r.Id == model.Id);

            if (rating == null)
                return RedirectToAction(nameof(Index)); //change to "Restaurant" ratings later
            _context.Ratings.Remove(rating);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Restaurant), new { id = model.RestaurantId }); //again, probably want to change to display of RatinglistItems for "Restaurant"
        }

    }
}