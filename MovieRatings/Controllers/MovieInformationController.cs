using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MovieRatings.DomainTypes.NonCoreDTOs;
using MovieRatings.EFRepository.Interfaces;
using MovieRatings.Services.Interfaces;

namespace MovieRatings.Controllers
{

    [ApiController]
    public class MovieInformationController : ControllerBase
    {
        private readonly IMovieInformationService _movieInformationService;

        public MovieInformationController(IMovieInformationService movieInformationService)
        {
            _movieInformationService = movieInformationService;
        }
        
        [HttpGet]
        [Route("api/movies")]
        public ActionResult<IEnumerable<MovieInformationDto>> GetMoviesByFilter([FromQuery] string title, [FromQuery] string yearOfRelease, [FromQuery] List<string> genres)
        {
 
            var movieInformation = _movieInformationService.GetMovieInformationByFilter(title, yearOfRelease, genres);

            if (movieInformation == null)
            {
                return BadRequest();
            }

            if (!movieInformation.Any())
            {
                return NotFound();
            }
            
            return Ok(movieInformation);
        }

        [HttpGet]
        [Route("api/getMoviesByRating")]
        public ActionResult<IEnumerable<MovieInformationDto>> GetTopFiveMoviesByRating()
        {
            var movieInformation = _movieInformationService.GetMoviesByAverageRating();

            if (movieInformation == null)
            {
                return BadRequest();
            }

            if (!movieInformation.Any())
            {
                return NotFound();
            }

            return Ok(movieInformation);
        }

        [HttpGet]
        [Route("api/getMoviesByRating/{userId}")]
        public ActionResult<IEnumerable<MovieInformationDto>> GetTopFiveMoviesRatedByUser(int userId)
        {

            var movieInformation = _movieInformationService.GetMoviesByAverageRatingForUser(userId);

            if (movieInformation == null)
            {
                return BadRequest();
            }

            if (!movieInformation.Any())
            {
                return NotFound();
            }

            return Ok(movieInformation);
        }

        [HttpPost]
        [Route("api/user/{userId}/movie/{movieId}/rating/{rating}/addOrUpdateMovieRating")]
        public ActionResult AddOrUpdateMovieRating(int userId, int movieId, int rating)
        {
            if (rating < 1 && rating > 5)
            {
                return BadRequest();
            }

            var success = _movieInformationService.AddOrUpdateUserRating(userId, movieId, rating);

            if (!success)
            {
                return NotFound();
            }

            return Ok(true);
        }


    }
}
