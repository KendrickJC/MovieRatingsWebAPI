using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.DomainTypes.CoreTypes
{
    public class UserRating
    {
        [Key]
        public int UserRatingId { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public List<User> Users { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public List<Movie> Movies { get; set; }
        [Range(1, 5, ErrorMessage = "Rating must between 1 to 5")]
        public int Rating { get; set; }
        public DateTime RatingDate { get; set; }
    }
}
