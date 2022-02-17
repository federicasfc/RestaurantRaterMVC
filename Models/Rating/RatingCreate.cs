using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace RestaurantRaterMVC.Models.Rating
{
    public class RatingCreate
    {
        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        [Required]
        [Range(1, 10)]
        public double Score { get; set; }

        //This is to hold our available Restaurant options for the dropdown selection
        public IEnumerable<SelectListItem> RestaurantOptions { get; set; } = new List<SelectListItem>();


    }
}