using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public  class UniKey
    {

        public static Guid GenerateUniqueKey()
        {
            var id  = Guid.NewGuid();
            return id;

        }
    }
}
