using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRaterMVC.Models.Restaurant
{
    public class RestaurantListItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        [Display(Name = "Average Score")]

        public double Score { get; set; }



    }
}