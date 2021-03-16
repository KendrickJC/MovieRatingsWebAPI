using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MovieRatings.DomainTypes.CoreTypes;

namespace MovieRatings.EFRepository.Interfaces
{
    public interface IUserRepository
    {
        User GetUserById(int userId);
    }
}
