using Xunit;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using BlobStorage.Providers.FileSystem;

namespace BlobStorage.Tests.Units.Providers
{
    public class FileSystemProviderTests : IDisposable
    {
        private readonly string _testBasePath;
        private readonly FileSystemProvider _provider;

        public FileSystemProviderTests()
        {
            // Arrange temporary folder for tests
            _testBasePath = Path.Combine(Path.GetTempPath(), "FileSystemProviderTests_" + Guid.NewGuid());
            Directory.CreateDirectory(_testBasePath);

            // Mock configuration
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Storage:LocalStorage:BasePath", _testBasePath }
            };

            IConfiguration configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            _provider = new FileSystemProvider(configuration);
        }

        [Fact]
        public async Task UploadAsync_ShouldCreateFile()
        {
            // Arrange
            var fileId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Hello Local Storage!");

          
            await _provider.UploadAsync(fileId, data);

          
            var filePath = Path.Combine(_testBasePath, fileId.ToString());
            Assert.True(File.Exists(filePath));
            var read = await File.ReadAllBytesAsync(filePath);
            Assert.Equal(data, read);
        }

        [Fact]
        public async Task GetAsync_ShouldReturnSameData()
        {
            
            var fileId = Guid.NewGuid();
            var expectedData = Encoding.UTF8.GetBytes("Retrieve test");
            var filePath = Path.Combine(_testBasePath, fileId.ToString());
            await File.WriteAllBytesAsync(filePath, expectedData);

          
            var result = await _provider.GetAsync(fileId);

         
            Assert.NotNull(result);
            Assert.Equal(expectedData, result);
        }

        [Fact]
        public async Task RetrieveBlobAsync_ShouldReturnBlobMetadata()
        {
        
            var fileId = Guid.NewGuid();
            var data = Encoding.UTF8.GetBytes("Metadata Test");
            var filePath = Path.Combine(_testBasePath, fileId.ToString());
            await File.WriteAllBytesAsync(filePath, data);

         
            var result = await _provider.RetrieveBlobAsync(fileId);

        
            Assert.Equal(fileId, result.Id);
            Assert.Equal(data, result.Data);
            Assert.Equal(data.Length, result.Size);
            Assert.True(result.CreatedAtUtc <= DateTimeOffset.UtcNow);
        }

        [Fact]
        public async Task DeleteAsync_ShouldRemoveFile()
        {
          
            var fileId = Guid.NewGuid();
            var filePath = Path.Combine(_testBasePath, fileId.ToString());
            await File.WriteAllBytesAsync(filePath, Encoding.UTF8.GetBytes("Delete Test"));

      
            var result = await _provider.DeleteAsync(fileId);

         
            Assert.True(result);
            Assert.False(File.Exists(filePath));
        }

        [Fact]
        public async Task DeleteAsync_ShouldReturnFalse_WhenFileDoesNotExist()
        {
        
            var nonExistentId = Guid.NewGuid();

        
            var result = await _provider.DeleteAsync(nonExistentId);

         
            Assert.False(result);
        }

        public void Dispose()
        {
            if (Directory.Exists(_testBasePath))
                Directory.Delete(_testBasePath, recursive: true);
        }
    }
}
