

using BlobStorage.Core.DTOs.Blob;
using BlobStorage.Core.Global;
using BlobStorage.Core.Interfaces;
using BlobStorage.Providers.FTP.Settings;
using FluentFTP;
using Microsoft.Extensions.Configuration;
using static BlobStorage.Core.Global.RefrencesName;
using System.Runtime.CompilerServices;

namespace BlobStorage.Providers.FTP {

    public class FTPBlobProvider : IObjectStorage
    {
        public string Name => "FTP";

        public EnRefrencesName StorageType => EnRefrencesName.FTPServer;

        public string Reference =>$"{_basePath}/{_objectKey}";

        private string? _objectKey;
        private readonly string _host;
        private readonly int _port;
        private readonly string _username;
        private readonly string _password;
        private readonly string _basePath;

        public FTPBlobProvider(IConfiguration config)
        {
            _host = config["Storage:FTP:Host"];
            _port = int.Parse(config["Storage:FTP:Port"] ?? "21");
            _username = config["Storage:FTP:Username"];
            _password = config["Storage:FTP:Password"];
            _basePath = config["Storage:FTP:BasePath"] ?? "/";
        }

        public async Task<bool> DeleteAsync(Guid Id)
        {
            string remotePath = $"{_basePath}/{Id}";
            using var client = await EstablishedConnection.ConnectAsyncFTP(_host, _port, _username, _password);


            bool exists = await client.FileExists(remotePath);
            if (!exists) { return false; }

            await client.DeleteFile(remotePath);
            await client.Disconnect();
           
            
            return true;

        }

        public async Task<byte[]> GetAsync(Guid Id)
        {
            string remotePath = $"{_basePath}/{Id}";

            using var client = await EstablishedConnection.ConnectAsyncFTP(_host, _port, _username, _password);
            var data = await  client.DownloadBytes(remotePath , 0 ,null , CancellationToken.None);

            await client.Disconnect();
            return data ?? Array.Empty<byte>(); 
        }

        public async Task<RetrieveBlobDTO> RetrieveBlobAsync(Guid Id)
        {
            string remotePath = $"{_basePath}/{Id}";

            using var client = await EstablishedConnection.ConnectAsyncFTP(_host, _port, _username, _password);

            var item = await  client.GetObjectInfo(remotePath);
            if (item == null) return null;

            byte[] data = await client.DownloadBytes(remotePath , 0  , null  ,CancellationToken.None);
            await client.Disconnect();

            return new RetrieveBlobDTO
            {
                Id = Id,
                Data = data,
                Size = data.Length  ,
                CreatedAtUtc = item.Modified


            };

        }
        
        public async Task UploadAsync(Guid Id, byte[] data)
        {
            string remotePath = $"{_basePath}/{Id}";
           
           if(Id != Guid.Empty) _objectKey = Id.ToString()  ;
            using var client = await EstablishedConnection.ConnectAsyncFTP(_host , _port ,_username , _password);
            await client.UploadBytes(data, remotePath, FtpRemoteExists.Overwrite, true );
           
            await client.Disconnect();

          
        }


    }
}