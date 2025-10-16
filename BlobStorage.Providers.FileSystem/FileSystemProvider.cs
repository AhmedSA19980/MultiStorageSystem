using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using static BlobStorage.Core.Global.RefrencesName;

namespace BlobStorage.Providers.FileSystem;

public class FileSystemProvider : IObjectStorage
{

    public string Name => "LocalStorage";
    public EnRefrencesName StorageType => EnRefrencesName.LocalStorage;
    public string Reference => _basePath;

    private readonly string _basePath;


    public FileSystemProvider(IConfiguration config)
    {

        _basePath = config["Storage:LocalStorage:BasePath"];

    }

    public async Task<bool> DeleteAsync(Guid key)
    {
      
        var filePath = Path.Combine(_basePath, $"{key}");
        if (File.Exists(filePath)) {
            File.Delete(filePath);
                return true;
        };
        return false;
    }

    public async Task<byte[]> GetAsync(Guid key)
    {

        var filePath = Path.Combine(_basePath, key.ToString());
        return await File.ReadAllBytesAsync(filePath);

    }



    public async Task UploadAsync(Guid key, byte[] data)
    {
        var filePath = Path.Combine(_basePath, key.ToString());
        var folderPath = Path.GetDirectoryName(filePath);

        Folder.CreateFolderIfDoesNotExist(folderPath);

        await File.WriteAllBytesAsync(filePath, data);
    }

      public async Task<RetrieveBlobDTO> RetrieveBlobAsync(Guid Id)
    {
        var filePath = Path.Combine(_basePath, $"{Id}");
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"Blob with reference '{Id}' not found in local storage {_basePath}");
             
        };

        byte[] fileBytes = await File.ReadAllBytesAsync(filePath);

        DateTimeOffset createdAt = File.GetCreationTimeUtc(filePath);
      
        var result = new RetrieveBlobDTO
        {
            Id = Id,
            Data = fileBytes,
            Size = fileBytes.Length,
            CreatedAtUtc = createdAt
        };
        return result;


   
    }
}
