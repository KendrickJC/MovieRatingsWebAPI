using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace MovieRatings.DomainTypes.CoreTypes
{
    public class MovieGenre
    {
        [Key]  
        public int MovieGenreId { get; set; }
        public int MovieId { get; set; }
        [ForeignKey("MovieId")]
        public List<Movie> Movies { get; set; }
        public int GenreId { get; set; }
        [ForeignKey("GenreId")]
        public List<Genre> Genres { get; set; }
    }
}
