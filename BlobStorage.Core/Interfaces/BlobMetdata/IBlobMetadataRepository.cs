using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Service.DTOs.Blobmetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobStorage.Core.Interfaces.BlobMetdata
{
    public  interface IBlobMetadataRepository<T> : IWriteRepository<T> where T : class 
    {
      
       Task<BlobMetadataDTO> GetMetadataByBlobIdAsync(Guid Id);


    }
}
