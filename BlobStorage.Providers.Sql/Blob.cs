using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using static BlobStorage.Core.Global.RefrencesName;


namespace BlobStorage.Providers.Sql
{
    public class SqlBlobProvider : IObjectStorage
    {

        public string Name => "SqlDatabase";
        public EnRefrencesName StorageType => EnRefrencesName.dbSql;
        private string? _tableReference;
        public string Reference {


            get {
            
                if(_tableReference == null)
                {
                    var EntityType = _context.Model.FindEntityType(typeof(Blob));
                    var schema = EntityType.GetSchema();
                    var tableName = EntityType.GetTableName();

                    _tableReference = string.IsNullOrEmpty(schema) ? "TableName->"+ tableName : $"{schema}.{tableName}";
                  
                }
                return _tableReference;
            }
            
        }

     

      


        private readonly AppDbContext _context;

        public SqlBlobProvider(AppDbContext context)
        {
            _context = context;
        }


        public async Task UploadAsync(Guid id , byte[] data)
        {

            var blob = new Blob
            {
                Id= id,
                Data = data
            };

            await _context.Blobs.AddAsync(blob);
            await _context.SaveChangesAsync();


        }


  

        public async Task<byte[]> GetAsync(Guid blobId)
        {
           
            var blob = await _context.Blobs.FirstOrDefaultAsync(b => b.Id== blobId);

            if (blob == null)
            {
                throw new FileNotFoundException($"Blob with reference '{blobId}' not found.");
            }

            return blob.Data;
        }


        public  async Task<bool> DeleteAsync(Guid blobId)
        {
            

            var blob = await _context.Blobs.FirstOrDefaultAsync(b => b.Id == blobId);

            if (blob == null)
            {
                //throw new FileNotFoundException($"Blobs with reference '{blobId}' not found.");
                return false;
            }
         
        
            _context.Blobs.Remove(blob);
            await _context.SaveChangesAsync(); 


            return  true;
        }

        public async Task<RetrieveBlobDTO> RetrieveBlobAsync(Guid BlobId)
        {
            var result = await(from BM in _context.BlobMetadata
                               join B in _context.Blobs on BM.BlobId equals B.Id
                               where (BM.BlobId == BlobId)
                               select new RetrieveBlobDTO
                               {

                                   Id = BM.BlobId,
                                   Data = B.Data,
                                   Size = BM.SizeInBytes,
                                   CreatedAtUtc = BM.Created_at }).FirstOrDefaultAsync();

            if (result == null)
            {

             
                throw new FileNotFoundException($"Blob with reference '{BlobId}' not found.");
            }


            return result;

        }
    }
}
