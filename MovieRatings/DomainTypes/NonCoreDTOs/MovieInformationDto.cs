using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;

namespace MovieRatings.DomainTypes.NonCoreDTOs
{
    public class MovieInformationDto
    {
        public int MovieId { get; set; }
        public string Title { get; set; }
        public string YearOfRelease { get; set; }
        public int RunningTime { get; set; }
        public List<string> Genres { get; set; }
        public double AverageRating { get; set; }
    }
}
