using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Text;
using System;
using System.Threading.Tasks;
using AutoMapper;
using BlobStorage.Core.Interfaces.BlobMetdata;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Models;
using BlobStorage.ProviderFactory;
using BlobStorage.Providers.FileSystem;
using BlobStorage.Providers.FTP;
using BlobStorage.Providers.S3;
using BlobStorage.Providers.Sql;
using BlobStorage.Service;

public class BlobServiceIntegrationTests
{
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;
    private readonly ProviderFactory _factory;
    private readonly BlobMetadataService _metadataService;

    public BlobServiceIntegrationTests()
    {
        // 1. Build configuration for a specific provider
        var inMemorySettings = new Dictionary<string, string>
        { 
            ["Storage:Provider"] = "FTP", // change to S3 / FTP / LocalStorage
            ["Storage:S3:Bucket"] = "set_bucketname",
            ["Storage:S3:AccessKey"] = "set_username",
            ["Storage:S3:SecretKey"] = "set_pass",
            ["Storage:S3:Endpoint"] = "http://localhost:9000",
            ["Storage:FTP:Host"] = "127.0.0.1",
            ["Storage:FTP:Port"] = "21",
            ["Storage:FTP:Username"] = "set_username",
            ["Storage:FTP:Password"] = "set_pass",
            [ "Storage:FTP:BasePath"] =  "/" ,
            ["Storage:LocalStorage:BasePath"] = "" //set dir
        };

        _config = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        //  Setup EF Core (use InMemory for test)
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase("set_your_db_connection_address")
            .Options;
        _dbContext = new AppDbContext(options);

        //  Mock mapper (or configure real AutoMapper profile)
        var mapperMock = new Mock<IMapper>();
        _mapper = mapperMock.Object;

        //  Initialize all providers
        var providers = new IObjectStorage[]
        {
            new SqlBlobProvider(_dbContext),
            new FileSystemProvider(_config),
            new S3BlobProvider(_config),
            new FTPBlobProvider(_config)
        };

        _factory = new ProviderFactory(providers, providers[0]);

        //  Mock repository for metadata
        var repoMock = new Mock<IBlobMetadataRepository<BlobMetadata>>();
        _metadataService = new BlobMetadataService(repoMock.Object, _mapper, providers[0]);
    }

    [Fact]
    public async Task Upload_And_Retrieve_File_Using_Factory_And_Service()
    {
        
        var blobService = new BlobService(_factory, _config, _metadataService, _mapper);
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("Hello test!"));
        int userId = 1;

     
        var blobDto = await blobService.UploadAsync(base64, userId);
        var retrieved = await blobService.GetAsync(blobDto.Id);

     
        Assert.NotNull(retrieved);
        Assert.Equal(Encoding.UTF8.GetBytes("Hello test!"), retrieved);
    }

    [Fact]
    public async Task Retrieve_File_After_Upload_Works_Correctly()
    {
       
        var blobService = new BlobService(_factory, _config, _metadataService, _mapper);
        string base64 = Convert.ToBase64String(Encoding.UTF8.GetBytes("Data retrieval test"));
        int userId = 99;

      
        var blobDto = await blobService.UploadAsync(base64, userId);

        // Act — now retrieve the same file by Id
        var retrievedBytes = await blobService.GetAsync(blobDto.Id);

    
        Assert.NotNull(retrievedBytes);
        Assert.Equal(Encoding.UTF8.GetBytes("Data retrieval test"), retrievedBytes);
    }

}
