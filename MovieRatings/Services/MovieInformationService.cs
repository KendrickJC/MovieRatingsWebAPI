using System;
using System.Collections.Generic;
using System.Linq;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.DomainTypes.NonCoreDTOs;
using MovieRatings.EFRepository.Interfaces;
using MovieRatings.Services.Interfaces;

namespace MovieRatings.Services
{
    public class MovieInformationService : IMovieInformationService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieGenreRepository _movieGenreRepository;
        private readonly IUserRatingRepository _userRatingRepository;
        private readonly IUserRepository _userRepository;

        public MovieInformationService(IMovieRepository movieRepository, IMovieGenreRepository movieGenreRepository, IUserRatingRepository userRatingRepository, IUserRepository userRepository)
        {
            _movieRepository = movieRepository;
            _movieGenreRepository = movieGenreRepository;
            _userRatingRepository = userRatingRepository;
            _userRepository = userRepository;
        }

        public IEnumerable<MovieInformationDto> GetMovieInformationByFilter(string title, string yearOfRelease, List<string> genres)
        {
            if (string.IsNullOrEmpty(title) && string.IsNullOrEmpty(yearOfRelease) && (genres == null || !genres.Any()))
            {
                return null;
            }

            var movieInformationDto = new List<MovieInformationDto>();

            if (!string.IsNullOrEmpty(title))
            {
               var moviesFilteredByTitle = _movieRepository.GetMoviesByTitle(title);
               movieInformationDto = AddfilteredListToDto(moviesFilteredByTitle, movieInformationDto);
            }

            if (!string.IsNullOrEmpty(yearOfRelease))
            {
                if (!string.IsNullOrEmpty(title))
                {
                    movieInformationDto = movieInformationDto.Where(x => x.YearOfRelease.Equals(yearOfRelease)).ToList();
                }
                else
                {
                    var moviesFilteredByYearOfRelease = _movieRepository.GetMoviesByYearOfRelease(yearOfRelease);
                    movieInformationDto = AddfilteredListToDto(moviesFilteredByYearOfRelease, movieInformationDto);
                }
            }

            if (genres == null || !genres.Any()) return movieInformationDto;

            if (!string.IsNullOrEmpty(title) || !string.IsNullOrEmpty(yearOfRelease))
            {
                movieInformationDto = genres.Aggregate(movieInformationDto, (current, genre) => current.Where(el => el.Genres.Contains(genre)).ToList());
            }
            else
            {
                movieInformationDto = GetMoviesByGenre(genres, movieInformationDto);
            }

            return movieInformationDto;
        }



        public List<string> GetMovieGenreListByMovieId(int movieId)
        {
            return _movieGenreRepository.GetGenreByMovieId(movieId);
        }

        public double GetAverageRatingByMovieId(int movieId)
        {
            var averageRating = _userRatingRepository.GetAverageMovieRatingByMovieId(movieId);
            return Math.Round(averageRating * 2, MidpointRounding.AwayFromZero) / 2;
        }

        public double GetAverageRatingByMovieIdAndUserId(int movieId, int userId)
        {
            return _userRatingRepository.GetAverageMovieRatingByMovieIdAndUserId(movieId, userId);
        }

        public List<MovieInformationDto> GetMoviesByGenre( List<string> genres, List<MovieInformationDto> movieInformationDto)
        {
            var movieIds = _movieGenreRepository.GetMovieIdsByGenres(genres);
            var moviesFilteredByGenre = _movieRepository.GetMoviesByIds(movieIds);

            return AddfilteredListToDto(moviesFilteredByGenre, movieInformationDto);
        }

        public List<MovieInformationDto> AddfilteredListToDto(IEnumerable<Movie> filteredList, List<MovieInformationDto> movieInformationDto, int averageByUserId = -1)
        {
            var filteredListWithGenre = (from el in filteredList
                select new MovieInformationDto
                {
                    MovieId = el.MovieId,
                    Title = el.Title,
                    YearOfRelease = el.YearOfRelease.ToString(),
                    RunningTime = el.RunningTime,
                    Genres = GetMovieGenreListByMovieId(el.MovieId),
                    AverageRating = averageByUserId == -1 ? GetAverageRatingByMovieId(el.MovieId) : GetAverageRatingByMovieIdAndUserId(el.MovieId, averageByUserId)
                }).ToList();

            movieInformationDto.AddRange(filteredListWithGenre);
            return movieInformationDto;
        }

        public List<MovieInformationDto> GetMoviesByAverageRating()
        {
            var movieInformationDto = new List<MovieInformationDto>();
            var movies = _movieRepository.GetAllMovies();
            return AddfilteredListToDto(movies, movieInformationDto).OrderByDescending(movie=> movie.AverageRating).Take(5).ToList();
        }

        public List<MovieInformationDto> GetMoviesByAverageRatingForUser(int userId)
        {
            var movieInformationDto = new List<MovieInformationDto>();
            var movieIds = _userRatingRepository.GetMovieIdsRatedByUser(userId);
            var movies = _movieRepository.GetMoviesByIds(movieIds);
            return AddfilteredListToDto(movies, movieInformationDto, userId).OrderByDescending(movie => movie.AverageRating).Take(5).ToList();
        }

        public bool AddOrUpdateUserRating(int movieId, int userId, int rating)
        {
            if (_movieRepository.GetMovieById(movieId) == null || _userRepository.GetUserById(userId) == null)
            {
                return false;
            }

            var userRating = _userRatingRepository.GetUserRatingByUserIdAndMovieId(userId, movieId).FirstOrDefault();

            if (userRating != null && userRating.UserRatingId > 0)
            {
                _userRatingRepository.UpdateUserMovieRating(userId, movieId, rating, userRating);
                return true;
            }
            else
            {
                _userRatingRepository.AddUserMovieRating(userId, movieId, rating);
                return true;
            }
        }
    }
}
