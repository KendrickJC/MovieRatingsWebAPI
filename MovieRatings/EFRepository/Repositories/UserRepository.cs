using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;
using MovieRatings.EFRepository.Interfaces;

namespace MovieRatings.EFRepository.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(MovieRatingsContext context) : base(context)
        {
        }
        public User GetUserById(int userId)
        {
            return Get(userId);
        }
    }
}
