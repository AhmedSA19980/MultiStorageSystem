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







## 🗄️ Database Migration Commands

Use the following commands to create and apply migrations for the SQL Server database.

```bash
# Create the initial migration for the tables
dotnet ef migrations add NameYourMigrationHere --project BlobStorage.Providers.Sql --startup-project BlobStorage.Api --context AppDbContext

# Apply the migrations to create the database and tables
dotnet ef database update --project BlobStorage.Providers.Sql --startup-project BlobStorage.Api
