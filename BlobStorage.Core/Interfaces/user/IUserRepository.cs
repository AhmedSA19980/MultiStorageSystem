using BlobStorage.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Interfaces.user
{
    public interface IUserRepository<T>  :IUpdateableRepository<T>  where T : class
    {

         Task<T> GetUserByEmailAsync(string email);

        Task ChangePasswordAsync(T entity);
    }
}
