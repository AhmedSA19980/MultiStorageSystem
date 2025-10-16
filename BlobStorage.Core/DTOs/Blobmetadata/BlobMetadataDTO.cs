using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace BlobStorage.Service.DTOs.Blobmetadata
{
    public  class BlobMetadataDTO
    {
        public int Id { get; set; }
        public long Size { get; set; }

        public DateTimeOffset Created_at { get; set; }
        public string StorageLocationName { get; set; }
        public string LocationReference { get; set; }
        public string MimeType { get; set; }
        public Guid BlobId { get; set; }
        public int UserId { get; set; }
       
    }
}
