using AutoMapper;
using BlobStorage.Core.Interfaces;
using BlobStorage.Core.Interfaces.BlobMetdata;
using BlobStorage.Core.Interfaces.Factory;
using BlobStorage.Core.Interfaces.user;
using BlobStorage.Core.Models;
using BlobStorage.ProviderFactory;
using BlobStorage.Providers.FileSystem;
using BlobStorage.Providers.S3;
using BlobStorage.Providers.Sql;
using BlobStorage.Providers.FTP;
using BlobStorage.Service;
using BlobStorage.Service.DTOs.Blobmetadata;
using BlobStorage.Service.DTOs.User;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;


var builder = WebApplication.CreateBuilder(args);
var service = builder.Services;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();




builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters
    {
        // Validate the server that created the token
        ValidateIssuer = true,
        // Validate the recipient of the token is authorized to receive
        ValidateAudience = true,
        // Validate the token has not expired
        ValidateLifetime = true,
        // Validate the signing key
        ValidateIssuerSigningKey = true,

        // Set the valid issuer (your API's domain, from appsettings.json)
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        // Set the valid audience (your API's domain or client Id, from appsettings.json)
        ValidAudience = builder.Configuration["Jwt:Audience"],
        // Set the signing key (from appsettings.json)
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Optional: Add logging for debugging authentication failures
    options.Events = new JwtBearerEvents
    {
        OnAuthenticationFailed = context =>
        {
            Console.WriteLine($"Authentication failed: {context.Exception.Message}");
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            Console.WriteLine($"Token validated for user: {context.Principal.Identity.Name}");
            return Task.CompletedTask;
        }
    };
    //    Console.WriteLine($"--- JWT Validation Key (Program.cs): {builder.Configuration["Jwt:Key"]}");

});


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "renting property", Version = "v1" });

    // Define the security scheme for JWT Bearer authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme // replace "" to "Bearer"
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add a security requirement to use the "Bearer" scheme for all operations
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});




// Configure the database context
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

/*builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
*/
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());




builder.Services.AddScoped<IUserRepository<User>, UserRep>();
builder.Services.AddScoped<IObjectStorage, SqlBlobProvider>();
builder.Services.AddScoped<IObjectStorage, FileSystemProvider>();
builder.Services.AddScoped<IObjectStorage , S3BlobProvider>();

builder.Services.AddScoped<IObjectStorage, FTPBlobProvider>();
builder.Services.AddScoped<IBlobMetadataRepository<BlobMetadata>, BlobMetadataRep>();



builder.Services.AddScoped<IObjectStorageProviderFactory, ProviderFactory>();


builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BlobService>();
builder.Services.AddScoped<BlobMetadataService>();



var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();



// enviroment variable  :ConnectionStrings:DefaultConnection "Server=localhost;Database=BlobStorage;Trusted_Connection=True;TrustServerCertificate=True;"


