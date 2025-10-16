using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Models
{
    public class BlobMetadata
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public long SizeInBytes { get; set; }

        [Required]
        public DateTimeOffset Created_at { get; set; } 

        [Required]
        [Column(TypeName = "nvarchar(255)")]
        public string StorageLocationName { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(max)")]
        public string LocationReference { get; set; }

        [Required]
        [Column(TypeName = "nvarchar(100)")] // A reasonable length for MIME types
        public string MimeType { get; set; }

        [Required]
       
        public Guid BlobId { get; set; }
     

        [Required]
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
