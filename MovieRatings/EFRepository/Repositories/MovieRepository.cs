using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.EFRepository.Interfaces;

namespace MovieRatings.EFRepository.Repositories
{
    public class MovieRepository : Repository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieRatingsContext context) : base(context)
        {
        }

        public IEnumerable<Movie> GetMoviesByTitle(string title)
        {
            return MovieRatingsContext.Movies.Where(x => x.Title.Contains(title, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<Movie> GetMoviesByYearOfRelease(string yearOfRelease)
        {
            return MovieRatingsContext.Movies.Where(x => x.YearOfRelease.ToString().Equals(yearOfRelease, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        public IEnumerable<Movie> GetMoviesByIds(List<int> movieIds)
        {
            var movies = movieIds.Select(Get).ToList();
            return movies;
        }

        public IEnumerable<Movie> GetAllMovies()
        {
            return GetAll();
        }

        public Movie GetMovieById(int id)
        {
            return Get(id);
        }

        public MovieRatingsContext MovieRatingsContext => Context as MovieRatingsContext;
    }
}
