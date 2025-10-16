using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlobStorage.Providers.S3;
using BlobStorage.Providers.S3.AWS.Operations;
using static System.Runtime.InteropServices.JavaScript.JSType;
using BlobStorage.Providers.FTP;
using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
namespace BlobStorage.Tests.Units.Providers
{
    public class S3StorageIntegrationTests
    {
        private IConfiguration _configuration;
        public S3StorageIntegrationTests()
        {
            var settings = new Dictionary<string, string>
                {
                    {"Storage:S3:endpoint", "http://localhost:9000"},
                    {"Storage:S3:Bucket", "set_bucketname"},
                    {"Storage:S3:AccessKey", "set_username"},
                    {"Storage:S3:SecretKey", "set_pass"},
                    {"Storage:S3:Region", "us-east-1"},
                    {"Storage:S3:UsePathStyleEndpoint" , Convert.ToString(true)}

                };
            _configuration = new ConfigurationBuilder().AddInMemoryCollection(settings).Build();
        }
       
        [Fact]
        public async Task Retrieve_File_Should_ReturnObject_Froms3()
        {
            var provider = new S3BlobProvider(_configuration);
            Guid id = ConvertToGuid.ConvertStringToGuid(""); // pass actual id from s3 server
            var BlobData = await provider.RetrieveBlobAsync(id);
            var date = DateTime.Parse(BlobData.CreatedAtUtc.ToString());

            RetrieveBlobDTO blob = new RetrieveBlobDTO
            {
                Id = BlobData.Id,
                Data = BlobData.Data,
                Size = BlobData.Size,
                CreatedAtUtc = BlobData.CreatedAtUtc,

            };
            Assert.NotNull(BlobData);
            Assert.NotNull(blob);
            Assert.Equal(id, BlobData.Id);

        }

        [Fact]
        public async Task Upload_And_Retrieve_File_FromS3()
        {
            var provider = new S3BlobProvider(_configuration);
            Guid id = Guid.NewGuid();
            byte[] data = Encoding.UTF8.GetBytes("iVBORw0KGgoAAAANSUhEUgAAACAAAAAgCAYAAABzenr0AAAAAXNSR0IArs4c6QAAAjZJREFUWEftV0Fy2kAQ7BH+R8gpcMgbbJ6QB0SgP1ip3LBvqch/AOQP+AcorwBfbCUv8CFH0KRG2sWLIqRZFxQX741i2emZnukeCGc+dOb4eAfwpgp8W6xumOgSwBWAHKCMuPj9czy88aXUC8D32aq/7dHMBG6Klfe2PPoRDXMtEC8AcbpetgS3MbMkHIyODiBOVxOgzF5xOErC4VxxUT8Fcfo4A3iieRSAugpqCuJ0/QygrwSQJ+Hgo+buqQAgCQeqt1WXJBNlA5qkaZ6En6IjV+DMTehRBXUDyptqCuSyQoi8gnsDsJxWmhBcAixSLCcn5l8nl2JNU/ne8aJAHr++X13dfR1mTk/07cwLRT4+4EWBccCx/MgGNKMpn0vtN2IFYl5o6VBVoK4BTDySKggoCSzBGrxCpYadAJoFqBKafQCNXvEHwNOrhFPW2xa3Lk2tAFrUr8xuH8Ca6w1IzLdFgOxiI0sLUASYMNGYiSPbRwcBdNsvR8SVOTGVW9GeVVua5HsBanvCaMnSLi4tADrdL5PZrwDs1rOyCJK524RxumbXnMTa7QrXCKA7+12xy3F0tyQm/L3Y8GeX5zoAGWVimsr0HADgtXz8pz2SrRnbaVNfBAXm2x4tpY8OAFDtfodE74WJv9gmM/qwR4GiAp38tynuC0AP7j5Qp6CqTvBB7nTqgK+2vzomL+xi6k6ByV7KX8Y+OgDrF9JkMiWiA0KHAPPSAd/M6/edgLt/UE0ecZIK+ID/B6cjRDC7uXBSAAAAAElFTkSuQmCC");
            await provider.UploadAsync(id, data);

            var retrieved = await provider.GetAsync(id);

            Assert.NotNull(retrieved);
            Assert.Equal(data, retrieved);
        }


        [Fact]
        public async Task Delete_File_Should_Return_True_FromS3()
        {

            var provider = new S3BlobProvider(_configuration);
            var id = Guid.NewGuid();
            byte[] data = Encoding.UTF8.GetBytes("this is a test data!");
            await provider.UploadAsync(id, data);

            var deleted = await provider.DeleteAsync(id);
            Assert.True(deleted);
        }
    }
}
