using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantRaterMVC.Data
{
    public class Rating
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Restaurant")]

        public int RestaurantId { get; set; }

        [Required]
        public double Score { get; set; }


        //In module it says that virtual class is being added so that Restaurant data will be available when Ratings are queried, but I don't see why this is necessary in addition to the FK 
        //Is this to avoid having to do an include with the query?
        public virtual Restaurant Restaurant { get; set; }

    }
}