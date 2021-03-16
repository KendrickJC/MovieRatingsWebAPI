using System.ComponentModel.DataAnnotations;

namespace MovieRatings.DomainTypes.CoreTypes
{
    public class Movie
    {
        [Key]
        public int MovieId { get; set; }
        [Required]
        public string Title { get; set; }
        public int YearOfRelease { get; set; }
        public int RunningTime { get; set; }
    }
}
