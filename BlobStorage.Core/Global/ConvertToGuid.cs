using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public class ConvertToGuid
    {
        public static Guid ConvertStringToGuid(string value)
        {
           
            if (Guid.TryParse(value, out Guid guidValue))
            {
                return guidValue;
                
            }
            else
            {
               
                return Guid.Empty;  
            }
        }
    }
}
