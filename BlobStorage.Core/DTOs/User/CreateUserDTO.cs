using BlobStorage.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Service.DTOs.User
{
    public  class CreateUserDTO
    {

     
        public string Username { get; set; }
        public string Email { get; set; }
        [Required]
        [MinLength(4)]
        public string Password { get; set; }
        public DateTime DateOfBirth { get; set; }
        
    }

}
