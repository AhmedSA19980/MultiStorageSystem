using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Service.DTOs.Auth
{
    public class AuthResponseDTO
    {
        public string AccessToken { get; set; }
       
        public int ExpiresIn { get; set; } 
    }
}
