# MultiStoreSystem

This project implements a simple yet extensible Object Storage System that unifies multiple storage backends under a single, consistent interface.

It allows developers to interact with various storage services—such as cloud-based, database, or local file systems—without needing to change their application logic.

Supported Storage Backends
* Amazon S3–Compatible Storage Service: (Using MinIO for local S3 emulation)
* Database Table: Store and retrieve binary data directly from a database as well as store Metadata , and User.
* Local File System: Save and access files on the host machine.
* FTP (Optional): For remote file storage via traditional file transfer protocol.



## 🧩 Tech Stack

| Category            | Technologies Used |
|----------------------|-------------------|
| **Backend**          | C#, .NET Core, RESTful API |
| **Authentication**   | JWT (JSON Web Tokens) |
| **Database**         | SQL Server, Entity Framework Core |
| **Object Storage**   | MinIO (S3-compatible) via Docker |
| **File Transfer**    | FluentFTP + FileZilla (FTP Server) |
| **Testing**          | xUnit.net |






first set your configuration on BlobStorage.Api/appsettings.json 
- save changes
run ```bash dotnet clean```
 
run ```bash dotnet build```

then make sure you current database address by run : ```bash dotnet ef dbcontext info  --project BlobStorage.Providers.Sql --startup-project BlobStorage.Api ```



## 🗄️ Database Migration Commands

Use the following commands to create and apply migrations for the SQL Server database.

```bash
# Create the initial migration for the tables
dotnet ef migrations add NameYourMigrationHere --project BlobStorage.Providers.Sql --startup-project BlobStorage.Api --context AppDbContext

# Apply the migrations to create the database and tables
dotnet ef database update --project BlobStorage.Providers.Sql --startup-project BlobStorage.Api
```

## 🗄️ Run MinIO (recommended: Docker)

```bash
docker run -p 9000:9000 -p 9001:9001 --name minio \
  -e "MINIO_ROOT_USER=setyouradminusername" \
  -e "MINIO_ROOT_PASSWORD=setyouradminpass" \
  -v /tmp/minio-data:/data \
  -d minio/minio server /data --console-address ":9001"
```




### Note: create bucket

3. Configure your app to use MinIO

```bash
"S3": {
  "Endpoint": "yourminioendpoint",    // MinIO endpoint (HTTP) for example :http://localhost:9000
  "Bucket": "setbucketname",
  "AccessKey": "setadminusername",
  "SecretKey": "setadminpass",
  "Region": "us-east-1",
  "UsePathStyleEndpoint": true
}
```
4. Make sure S3 signing + endpoint code compatible with MinIO.
 
For example : http://localhost:9000/{bucket}/{key}



## 🗄️ Configure App using FTP 

install fluentftp package 

```bash
dotnet add package FluentFTP
```

Note: install FileZilla server .



