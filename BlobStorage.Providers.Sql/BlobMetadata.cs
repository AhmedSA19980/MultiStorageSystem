using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.BlobMetdata;
using BlobStorage.Core.Models;
using BlobStorage.Service.DTOs.Blobmetadata;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace BlobStorage.Providers.Sql
{
    public class BlobMetadataRep : IBlobMetadataRepository<BlobMetadata>
    {


        private readonly AppDbContext _context;

        public BlobMetadataRep(AppDbContext context)
        {
            _context = context;
        }


        public async Task<BlobMetadata> GetByIdAsync(int id)
        {

            return await _context.BlobMetadata.FindAsync(id);
        }

        public async Task<BlobMetadataDTO> GetMetadataByBlobIdAsync(Guid blobId)
        {
            var result = await  _context.BlobMetadata.Where(m => m.BlobId == blobId).Select(m => new BlobMetadataDTO
            {
               Id = m.Id,
                BlobId = m.BlobId,
                Size = m.SizeInBytes,
                StorageLocationName = m.StorageLocationName,
                LocationReference = m.LocationReference,
                MimeType = m.MimeType,
                Created_at = m.Created_at,
                UserId = m.UserId,

            }).FirstOrDefaultAsync();
               
            if (result == null)
            {
             
                throw new FileNotFoundException($"Blob Metadata with reference '{blobId}' not found.");
            }

            return result;

        }


        public async Task AddAsync(BlobMetadata metadata)
        {
       
            await _context.BlobMetadata.AddAsync(metadata);
            await _context.SaveChangesAsync();
        }


    }
}