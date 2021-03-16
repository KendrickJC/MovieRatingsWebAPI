using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;

namespace MovieRatings.EFRepository.Interfaces
{
    public interface IUserRatingRepository
    {
        double GetAverageMovieRatingByMovieId(int movieId);
        double GetAverageMovieRatingByMovieIdAndUserId(int movieId, int userId = -1);
        List<int> GetMovieIdsRatedByUser(int userId);
        List<UserRating> GetUserRatingByUserIdAndMovieId(int userId, int movieId);
        void UpdateUserMovieRating(int userId, int movieId, int rating, UserRating userRating);
        void AddUserMovieRating(int userId, int movieId, int rating);
    }
}
