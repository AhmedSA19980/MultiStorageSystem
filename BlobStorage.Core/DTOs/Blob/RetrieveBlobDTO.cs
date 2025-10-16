using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlobStorage.Core.DTOs.Blob
{

   
    public  class RetrieveBlobDTO
    {


        public Guid Id { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }

        [JsonIgnore]
        public DateTimeOffset CreatedAtUtc { get; set; }
        public string Created_at => CreatedAtUtc.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss'Z'");
    }
}
