using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.Blobmetadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static BlobStorage.Core.Global.RefrencesName;

namespace BlobStorage.Core.Interfaces
{

     
    
    public interface  IObjectStorage
    {

        string Name { get; }                   // Provider identifier
        EnRefrencesName StorageType { get; }   // provider type
        string Reference { get; }
        Task UploadAsync(Guid key , byte[] data);
        Task<byte[]> GetAsync(Guid key);
        Task<RetrieveBlobDTO> RetrieveBlobAsync(Guid Id);
        Task<bool>  DeleteAsync(Guid key);
       

    }
}
