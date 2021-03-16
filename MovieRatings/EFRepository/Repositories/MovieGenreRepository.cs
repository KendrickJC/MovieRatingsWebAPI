using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.DomainTypes.NonCoreDTOs;
using MovieRatings.EFRepository.Interfaces;
using Remotion.Linq.Clauses;

namespace MovieRatings.EFRepository.Repositories
{
    public class MovieGenreRepository : Repository<MovieGenre>, IMovieGenreRepository
    {
        public MovieGenreRepository(MovieRatingsContext context) : base(context)
        {
        }

        public List<string> GetGenreByMovieId(int movieId)
        {
            var genres = (
                from g in MovieRatingsContext.Genres
                join mg in MovieRatingsContext.MovieGenres on g.GenreId equals mg.GenreId
                where mg.MovieId.Equals(movieId)
                select g.GenreName
            ).ToList();

            return genres;
        }

        public List<int> GetGenreIdByName(List<string> genres)
        {
            var genreIds = new List<int>();

            foreach (var genre in genres)
            {
                genreIds = (from g in MovieRatingsContext.Genres
                    where g.GenreName.Equals(genre,StringComparison.OrdinalIgnoreCase)
                    select g.GenreId
                ).ToList();
            }

            return genreIds;
        }

        public List<int> GetMovieIdsByGenres(List<string> genres)
        {
            var genreIds = GetGenreIdByName(genres);

            var movieIds = new List<int>();

            foreach (var genreId in genreIds)
            {
                movieIds = (from mg in MovieRatingsContext.MovieGenres
                        where mg.GenreId.Equals(genreId)
                        select mg.MovieId
                    ).Distinct().ToList();
            }

            return movieIds;
        }

        public MovieRatingsContext MovieRatingsContext => Context as MovieRatingsContext;
    }
}
