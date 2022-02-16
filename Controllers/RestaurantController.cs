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
        } //works

        //GetById basically - sends user to a view with  restaurant details

        [ActionName("Details")] //Why not just name this method RestautantDetails and have the action name also be RestaurantDetails? Just to show off that they can differ and be manipulated accordingly?
        public async Task<IActionResult> Restaurant(int id)
        {
            Restaurant restaurant = await _context.Restaurants
            .Include(r => r.Ratings)
            .FirstOrDefaultAsync(r => r.Id == id);

            //if restaurant is null just go back to index; note that the action here is referencing the method name, which connects to the index view in Restauran folder
            if (restaurant == null)
                return RedirectToAction(nameof(Index));

            RestaurantDetail restaurantDetail = new RestaurantDetail()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location,
                Score = restaurant.Score
            };

            //Return Restaurant view (connected again by corresponding the method and view names) with the restaurantDetail's info inputed
            //In this case, the name of the View won't match the actual method name(restaurant) because we specified that the action name would be "details" (and view will be called details.cshtml)
            return View(restaurantDetail);


        }//works

        //Get method for Create
        //Will return the Html form view that we'll use to create restaurants
        public async Task<IActionResult> Create()
        {
            return View();
        }

        //PostMethod for Create
        //will take in the form data from the create view that EntityFramework will deserialize into the right format (C# RestaurantCreate model object)
        [HttpPost]
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
        }//works

        //Get for Edit method
        //Like usual, pulls out the restaurant from the db  by id and if it exists, returns the Edit view with the new restaurantEdit info inputed
        public async Task<IActionResult> Edit(int id)
        {
            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
                return RedirectToAction(nameof(Index));

            RestaurantEdit restaurantEdit = new RestaurantEdit()
            {
                Id = restaurant.Id,
                Name = restaurant.Name,
                Location = restaurant.Location
            };

            return View(restaurantEdit);

        }

        //Post endpoint for Edit method...Why not put??

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RestaurantEdit model)
        {

            if (!ModelState.IsValid)
            {
                return View(ModelState);
            }

            Restaurant restaurant = await _context.Restaurants.FindAsync(id);

            if (restaurant == null)
                return RedirectToAction(nameof(Index));

            restaurant.Name = model.Name;
            restaurant.Location = model.Location;
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", new { id = restaurant.Id });
        }//works, but still a little unclear on how the get and post are interacting with each other, and why not put.


    }
}