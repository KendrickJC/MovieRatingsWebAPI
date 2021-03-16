using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MovieRatings.DomainTypes.CoreTypes;

namespace MovieRatings.EFRepository
{
    public class MovieRatingsContext : DbContext
    {
        public MovieRatingsContext(DbContextOptions<MovieRatingsContext> options) : base(options)
        {
        }

        public DbSet<Movie> Movies { get; set; }
        public DbSet<MovieGenre> MovieGenres { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<UserRating> UserRatings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
