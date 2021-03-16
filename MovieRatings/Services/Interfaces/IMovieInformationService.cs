using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.NonCoreDTOs;

namespace MovieRatings.Services.Interfaces
{
    public interface IMovieInformationService
    {
        IEnumerable<MovieInformationDto> GetMovieInformationByFilter(string title, string yearOfRelease, List<string> genre);
        List<MovieInformationDto> GetMoviesByAverageRating();
        List<MovieInformationDto> GetMoviesByAverageRatingForUser(int userId);
        bool AddOrUpdateUserRating(int movieId, int userId, int rating);
    }
}
