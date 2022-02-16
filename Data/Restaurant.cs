using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRaterMVC.Data
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]

        public string Name { get; set; }

        [Required]
        [MaxLength(100)]

        public string Location { get; set; }


        public double Score
        {
            get
            {
                //if the number of ratings is greater than 0, return the average of scores from the ratings; else, return 0
                return Ratings.Count > 0 ? Ratings.Select(r => r.Score).Sum() / Ratings.Count : 0;
            }
        }

        public virtual List<Rating> Ratings { get; set; } = new List<Rating>();

        //setting this equal to an empty list so that it isn't set to null; however, I thought this wasn't done with FK properties


    }
}