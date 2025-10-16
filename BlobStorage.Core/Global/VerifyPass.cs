using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Global
{
    public  class VerifyPass
    {
        public static bool  VerifyPassword( string Password , string InputPassword) {

           
            return Password == HashPass.hashPassword(InputPassword);
            
        }
    }
}
