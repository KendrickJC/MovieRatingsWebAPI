using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.EFRepository.Interfaces;

namespace MovieRatings.EFRepository.Repositories
{
    public class UserRatingRepository : Repository<UserRating>, IUserRatingRepository
    {
        public UserRatingRepository(MovieRatingsContext context) : base(context)
        {
        }

        public double GetAverageMovieRatingByMovieId(int movieId)
        {
            var intRatings = (from u in MovieRatingsContext.UserRatings
                where u.MovieId.Equals(movieId)
                select u.Rating).ToList();

            return intRatings.Sum() / (double)intRatings.Count;
        }

        public double GetAverageMovieRatingByMovieIdAndUserId(int movieId, int userId = -1)
        {
            var intRatings = (from u in MovieRatingsContext.UserRatings
                              where (u.MovieId.Equals(movieId) && u.UserId.Equals(userId))
                              select u.Rating).ToList();

            return intRatings.Sum() / (double)intRatings.Count;
        }

        public List<int> GetMovieIdsRatedByUser(int userId)
        {
            var movieIds = (from ur in MovieRatingsContext.UserRatings
                        where ur.UserId.Equals(userId)
                        select ur.MovieId
                    ).Distinct().ToList();

            return movieIds;
        }

        public List<UserRating> GetUserRatingByUserIdAndMovieId(int userId, int movieId)
        {
            return GetAll().Where(x => x.UserId.Equals(userId) && x.MovieId.Equals(movieId)).ToList();
        }

        public void UpdateUserMovieRating(int userId, int movieId, int rating, UserRating userRating)
        {
            if (userRating.Rating == rating) return;

            userRating.Rating = rating;
            userRating.RatingDate = DateTime.Now;
            MovieRatingsContext.SaveChanges();
        }

        public void AddUserMovieRating(int userId, int movieId, int rating)
        {
            var userRatingToAdd = new UserRating
            {
                MovieId = movieId,
                UserId = userId,
                Rating = rating,
                RatingDate = DateTime.Now
            };

            Add(userRatingToAdd);
            MovieRatingsContext.SaveChanges();
        }

        public MovieRatingsContext MovieRatingsContext => Context as MovieRatingsContext;
    }
}
