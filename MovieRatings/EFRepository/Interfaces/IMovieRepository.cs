using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;

namespace MovieRatings.EFRepository.Interfaces
{
    public interface IMovieRepository
    {
        IEnumerable<Movie> GetMoviesByTitle(string title);
        IEnumerable<Movie> GetMoviesByYearOfRelease(string yearOfRelease);
        IEnumerable<Movie> GetMoviesByIds(List<int> movieIds);
        IEnumerable<Movie> GetAllMovies();
        Movie GetMovieById(int id);
    }
}
