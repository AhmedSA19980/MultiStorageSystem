using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Interfaces
{
    public  interface IReadRepository<T> where T : class
    {
        Task<T> GetByIdAsync(int id);
      
       
    }



}
