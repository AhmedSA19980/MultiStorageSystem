using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Interfaces
{
 
        public interface IUpdateableRepository<T> : IWriteRepository<T> where T : class
        {

            Task UpdateAsync(T entity);

        }

    
}
