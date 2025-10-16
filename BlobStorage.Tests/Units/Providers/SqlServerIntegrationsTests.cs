using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobStorage.Core.Global;
using BlobStorage.Core.Models;
using BlobStorage.Providers.Sql;
using Microsoft.EntityFrameworkCore;




namespace BlobStorage.Tests.Units.Providers
{

    public class SqlBlobProviderTests
    {
        private readonly AppDbContext _context;
        private readonly SqlBlobProvider _provider;

        public SqlBlobProviderTests()
        {
            // 1. Configure EF Core InMemory database
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase("set_your_db_connection_address")
                .Options;

            _context = new AppDbContext(options);

           
            _provider = new SqlBlobProvider(_context);
        }

        [Fact]
        public async Task UploadAsync_Should_Save_Blob_To_Database()
        {
          
            var blobId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Test data for upload");

         
            await _provider.UploadAsync(blobId, data);
            var blobInDb = await _context.Blobs.FirstOrDefaultAsync(b => b.Id == blobId);

            
            Assert.NotNull(blobInDb);
            Assert.Equal(data, blobInDb.Data);
        }

        [Fact]
        public async Task GetAsync_Should_Return_Same_Data_As_Stored()
        {
            var blobId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Retrieve test data");
            await _provider.UploadAsync(blobId, data);

          
            var retrievedData = await _provider.GetAsync(blobId);

           
            Assert.Equal(data, retrievedData);
        }

        [Fact]
        public async Task GetAsync_Should_Throw_When_Blob_Not_Found()
        {
        
            var nonExistentId = Guid.NewGuid();

           
            await Assert.ThrowsAsync<FileNotFoundException>(
                () => _provider.GetAsync(nonExistentId)
            );
        }

        [Fact]
        public async Task DeleteAsync_Should_Remove_Blob_And_Return_True()
        {
           
            var blobId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Delete test data");
            await _provider.UploadAsync(blobId, data);

         
            var result = await _provider.DeleteAsync(blobId);

          
            Assert.True(result);
            Assert.Null(await _context.Blobs.FirstOrDefaultAsync(b => b.Id == blobId));
        }

        [Fact]
        public async Task DeleteAsync_Should_Return_False_When_Not_Found()
        {
           
            var nonExistentId = Guid.NewGuid();

            var result = await _provider.DeleteAsync(nonExistentId);

            Assert.False(result);
        }

        [Fact]
        public async Task RetrieveBlobAsync_Should_Return_Metadata_And_Data()
        {
           
            var blobId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Retrieve blob with metadata");

            var blob = new Blob { Id = blobId, Data = data };
            var metadata = new BlobMetadata
            {
                BlobId = blobId,
                SizeInBytes = data.Length,
                Created_at = DateTimeOffset.UtcNow,
                MimeType = FileType.GetFileMimeTypeFromBytes(data),
                LocationReference = "Table->blobs",
                StorageLocationName = "Sql"


            };

            _context.Blobs.Add(blob);
            _context.BlobMetadata.Add(metadata);
            await _context.SaveChangesAsync();

        
            var result = await _provider.RetrieveBlobAsync(blobId);

           
            Assert.NotNull(result);
            Assert.Equal(blobId, result.Id);
            Assert.Equal(data, result.Data);
            Assert.Equal(data.Length, result.Size);
            Assert.True(result.CreatedAtUtc <= DateTimeOffset.UtcNow);
        }

        [Fact]
        public async Task RetrieveBlobAsync_Should_Throw_When_Not_Found()
        {
            // Arrange
            var nonExistentId = Guid.NewGuid();

            // Act & Assert
            await Assert.ThrowsAsync<FileNotFoundException>(
                () => _provider.RetrieveBlobAsync(nonExistentId)
            );
        }
    }


}

