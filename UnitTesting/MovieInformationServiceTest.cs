using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Moq;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.DomainTypes.NonCoreDTOs;
using MovieRatings.EFRepository.Interfaces;
using MovieRatings.Services;
using NUnit.Framework;

namespace UnitTesting
{
    [TestFixture, Category("Unit test")]
    public class MovieInformationServiceTest
    {
        private MovieInformationService _movieInformationService;
        private Mock<IMovieRepository> _movieRepositoryMock;
        private Mock<IMovieGenreRepository> _movieGenreRepositoryMock;
        private Mock<IUserRatingRepository> _userRatingRepositoryMock;
        private Mock<IUserRepository> _userRepositoryMock;

        private MovieInformationDto _movieInformation1;
        private MovieInformationDto _movieInformation3;
        private List<string> _genres;

        private Movie _movie1, _movie2,_movie3, _movie4,_movie5,_movie6;

        [SetUp]
        public void TestBase()
        {
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _movieGenreRepositoryMock = new Mock<IMovieGenreRepository>();
            _userRatingRepositoryMock = new Mock<IUserRatingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();



            _movieInformationService = new MovieInformationService(_movieRepositoryMock.Object, _movieGenreRepositoryMock.Object, _userRatingRepositoryMock.Object, _userRepositoryMock.Object);

            _movie1 = new Movie
            {
                MovieId = 1,
                Title = "Toy Story",
                RunningTime = 100,
                YearOfRelease = 1995
            };

            _movie2 = new Movie
            {
                MovieId = 2,
                Title = "Avengers",
                RunningTime = 180,
                YearOfRelease = 2019
            };

            _movie3 = new Movie
            {
                MovieId = 3,
                Title = "The Lion King",
                RunningTime = 150,
                YearOfRelease = 1994
            };

            _movie4 = new Movie
            {
                MovieId = 4,
                Title = "The Shawshank Redemption",
                RunningTime = 150,
                YearOfRelease = 1996
            };

            _movie5 = new Movie
            {
                MovieId = 5,
                Title = "The Dark Knight",
                RunningTime = 150,
                YearOfRelease = 2008
            };

            _movie6 = new Movie
            {
                MovieId = 6,
                Title = "Batman vs Superman",
                RunningTime = 150,
                YearOfRelease = 2016
            };

            _movieInformation1 = new MovieInformationDto
            {
                MovieId = 1,
                Title = "Toy Story",
                RunningTime = 100,
                YearOfRelease = "1995",
                AverageRating = 4.5,
                Genres = new List<string>
                {
                    "Animation",
                    "Adventure",
                }
            };

            _movieInformation3 = new MovieInformationDto
            {
                MovieId = 3,
                Title = "The Lion King",
                RunningTime = 150,
                YearOfRelease = "1994",
                AverageRating = 3,
                Genres = new List<string>
                {
                    "Animation",
                    "Drama"
                }
            };

        }

        [Test]
        [TestCase("toy")]
        public void GetMovieInformationByFilter_Only_Title_Success(string title)
        {
            var testMovieList1 = new List<Movie> {_movie1};

            var expected = new List<MovieInformationDto> {_movieInformation1};
            var genres = new List<string>
            {
                "Animation",
                "Adventure",
            };
            _movieRepositoryMock.Setup(x => x.GetMoviesByTitle("toy")).Returns(testMovieList1);
            _movieGenreRepositoryMock.Setup(x => x.GetGenreByMovieId(1)).Returns(genres);
            _userRatingRepositoryMock.Setup(x => x.GetAverageMovieRatingByMovieId(1)).Returns(4.5);

            var result = _movieInformationService.GetMovieInformationByFilter(title, string.Empty, new List<string>());

            Assert.IsNotNull(result);
            Assert.That(result.Count(),Is.EqualTo(expected.Count));
        }

        [Test]
        [TestCase("1995")]
        public void GetMovieInformationByFilter_Only_YearOfRelease_Success(string yearOfRelease)
        {
            var testMovieList2 = new List<Movie> {_movie2};

            var expected = new List<MovieInformationDto> { _movieInformation1 };
            var genres = new List<string>
            {
                "Action",
                "Adventure"
            };

            _movieRepositoryMock.Setup(x => x.GetMoviesByYearOfRelease(yearOfRelease)).Returns(testMovieList2);
            _movieGenreRepositoryMock.Setup(x => x.GetGenreByMovieId(2)).Returns(genres);
            _userRatingRepositoryMock.Setup(x => x.GetAverageMovieRatingByMovieId(2)).Returns(5);

            var result = _movieInformationService.GetMovieInformationByFilter(string.Empty, yearOfRelease, new List<string>());

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expected.Count));
        }

        [Test]
        public void GetMovieInformationByFilter_Only_Genres_Success()
        {
            var testMovieList3 = new List<Movie> {_movie3};

            var expected = new List<MovieInformationDto> { _movieInformation3 };
            _genres = new List<string>
            {
                "Drama"
            };

            var movieIds = new List<int> {3};

            _movieGenreRepositoryMock.Setup(x => x.GetMovieIdsByGenres(_genres)).Returns(movieIds);
            _movieRepositoryMock.Setup(x => x.GetMoviesByIds(movieIds)).Returns(testMovieList3);
            _userRatingRepositoryMock.Setup(x => x.GetAverageMovieRatingByMovieId(3)).Returns(5);

            var result = _movieInformationService.GetMovieInformationByFilter(string.Empty, string.Empty, _genres);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(expected.Count));
        }

        [Test]
        public void GetMovieInformationByFilter_All_Filters_Success()
        {
            var testMovieList1 = new List<Movie> { _movie1 };

            var expected = new List<MovieInformationDto> { _movieInformation1 };

            _genres = new List<string>
            {
                "Animation"
            };

            var movieIds = new List<int> { 1 };

            _movieRepositoryMock.Setup(x => x.GetMoviesByTitle("toy")).Returns(testMovieList1);
            _movieRepositoryMock.Setup(x => x.GetMoviesByYearOfRelease("1995")).Returns(testMovieList1);
            _movieGenreRepositoryMock.Setup(x => x.GetMovieIdsByGenres(_genres)).Returns(movieIds);
            _movieGenreRepositoryMock.Setup(x => x.GetGenreByMovieId(1)).Returns(_movieInformation1.Genres);
            _movieRepositoryMock.Setup(x => x.GetMoviesByIds(movieIds)).Returns(testMovieList1);
            _userRatingRepositoryMock.Setup(x => x.GetAverageMovieRatingByMovieId(1)).Returns(4.5);

            var resultSuccess = _movieInformationService.GetMovieInformationByFilter("toy", "1995", _genres);
            var resultNotFound = _movieInformationService.GetMovieInformationByFilter("toy", "2000", _genres);

            Assert.IsNotNull(resultSuccess);
            Assert.That(resultSuccess.Count(), Is.EqualTo(expected.Count));
            Assert.That(resultNotFound.Count(), Is.EqualTo(0));
        }

        [Test]
        public void GetMoviesByAverageRating_Success()
        {
            var testAllMoviesList = new List<Movie> { _movie1, _movie2, _movie3, _movie4, _movie5, _movie6};

            _movieRepositoryMock.Setup(x => x.GetAllMovies()).Returns(testAllMoviesList);
            _movieGenreRepositoryMock.Setup(x => x.GetGenreByMovieId(1)).Returns(_movieInformation1.Genres);
            _userRatingRepositoryMock.Setup(x => x.GetAverageMovieRatingByMovieId(1)).Returns(4.5);

            var result = _movieInformationService.GetMoviesByAverageRating();

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public void GetMoviesByAverageRatingForUser_Success()
        {
            var movieIds = new List<int>{ 1,2,3,4,5,6};
            var testMovies = new List<Movie> { _movie1, _movie2, _movie3, _movie4, _movie5, _movie6 };
            _userRatingRepositoryMock.Setup(x => x.GetMovieIdsRatedByUser(1)).Returns(movieIds);
            _movieRepositoryMock.Setup(x => x.GetMoviesByIds(movieIds)).Returns(testMovies);

            var result = _movieInformationService.GetMoviesByAverageRatingForUser(1);

            Assert.IsNotNull(result);
            Assert.That(result.Count(), Is.EqualTo(5));
        }

        [Test]
        public void AddOrUpdateUserRating_Update_Success()
        {
            var user = new User{UserId = 1, UserEmail = "Test@test.com", UserName = "Test"};
            var userRating = new UserRating { UserRatingId = 1, UserId = 1, MovieId = 1, Rating = 5, RatingDate = DateTime.Now};

            var userRatingList = new List<UserRating> {userRating};

            _movieRepositoryMock.Setup(x => x.GetMovieById(1)).Returns(_movie1);
            _userRepositoryMock.Setup(x => x.GetUserById(1)).Returns(user);

            _userRatingRepositoryMock.Setup(x => x.GetUserRatingByUserIdAndMovieId(1, 1)).Returns(userRatingList);

            _userRatingRepositoryMock.Setup(x => x.UpdateUserMovieRating(1, 1, 4, userRating));

            _movieInformationService.AddOrUpdateUserRating(1, 1, 4);

            _userRatingRepositoryMock.Verify(x=> x.UpdateUserMovieRating(1,1,4,userRating),Times.Once);

        }

        [Test]
        public void AddOrUpdateUserRating_Add_Success()
        {
            var user = new User { UserId = 1, UserEmail = "Test@test.com", UserName = "Test" };

            var userRatingList = new List<UserRating>();

            _movieRepositoryMock.Setup(x => x.GetMovieById(1)).Returns(_movie1);
            _userRepositoryMock.Setup(x => x.GetUserById(1)).Returns(user);

            _userRatingRepositoryMock.Setup(x => x.GetUserRatingByUserIdAndMovieId(1, 1)).Returns(userRatingList);

            _userRatingRepositoryMock.Setup(x => x.AddUserMovieRating(1, 1, 4));

            var result = _movieInformationService.AddOrUpdateUserRating(1, 1, 4);

            _userRatingRepositoryMock.Verify(x => x.AddUserMovieRating(1, 1, 4), Times.Once);
            Assert.IsTrue(result);

        }

        [Test]
        public void AddOrUpdateUserRating_Null()
        {

            _movieRepositoryMock.Setup(x => x.GetMovieById(1));
            _userRepositoryMock.Setup(x => x.GetUserById(1));

            var result = _movieInformationService.AddOrUpdateUserRating(1, 1, 4);

            Assert.IsFalse(result);
        }
    }
}
