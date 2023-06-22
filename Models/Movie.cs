using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MovieOnlineBookingMVC.Models
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Image { get; set; }

        [Required]
        public string Rating { get; set; }

        [Required]
        public string Language { get; set; }



        [Required]
        public string Duration { get; set; }

        [Required]
        public string Genre { get; set; }

        [Required]
        public string Availability { get; set; }

        [Required]
        public string Cast { get; set; }

        [Required]
        public string Description { get; set; }


       


    }
}