using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MovieRatings.DomainTypes.CoreTypes
{
    public class Genre
    {
        [Key]
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set; }
    }
}
