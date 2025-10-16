using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Models
{
    public  class Blob
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column(TypeName = "varbinary(max)")]
        public byte[] Data { get; set; }
    }
}
