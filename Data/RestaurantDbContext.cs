using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace RestaurantRaterMVC.Data
{
    public class RestaurantDbContext : DbContext
    {
        //Constructor
        //Look into DbContextOptions, confused about this DI
        public RestaurantDbContext(DbContextOptions<RestaurantDbContext> options) : base(options)
        {

        }

        public DbSet<Restaurant> Restaurants { get; set; }

        public DbSet<Rating> Ratings { get; set; }
    }
}