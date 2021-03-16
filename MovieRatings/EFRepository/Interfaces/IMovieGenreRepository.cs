using System.Collections.Generic;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.DomainTypes.NonCoreDTOs;

namespace MovieRatings.EFRepository.Interfaces
{
    public interface IMovieGenreRepository
    {
        List<string> GetGenreByMovieId(int movieId);
        List<int> GetMovieIdsByGenres(List<string> genres);
    }
}
