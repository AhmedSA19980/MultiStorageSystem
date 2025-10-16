using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(50)")]
        public string Username { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string Email { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string Password { get; set; }

        public DateTime DateOfBirth { get; set; }
        // Navigation property for the collection of BlobMetadata
        public ICollection<BlobMetadata> BlobMetadata { get; set; }
    }
}
