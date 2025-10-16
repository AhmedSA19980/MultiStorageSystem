using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BlobStorage.Core.Global.RefrencesName;

namespace BlobStorage.Service.DTOs.Blobmetadata
{

 
    public  class AddBlobMetadataDTO
    {

    
        public long SizeInBytes { get; set; }
        public EnRefrencesName StorageLocationName { get; set; }
        public string LocationReference { get; set; }
        public string MimeType { get; set; }
   
        public Guid BlobId { get; set; }

        public int UserId { get; set; }
     
    }
}
