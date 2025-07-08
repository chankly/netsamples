using Azure.Storage.Blobs;
using Azure.Storage;
using HSoft.NetSamples.Api.Controllers;
using HSoft.NetSamples.Api.Data;
using HSoft.NetSamples.Api.Data.Repositories;
using HSoft.NetSamples.Api.Infrastructure.Exceptions;
using HSoft.NetSamples.Api.Infrastructure.Network.Downloads;
using Microsoft.EntityFrameworkCore;
using Microsoft.FeatureManagement;
using Microsoft.Extensions.Caching.Hybrid;

public partial class Program {
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddExceptionHandler<BadHttpRequestExceptionHandler>();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        builder.Services.AddProblemDetails();
        builder.Services.AddFeatureManagement()
                        .AddFeatureFilter<WorkingDayFeatureFilter>();

        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

        builder.Services.AddDbContext<MyShopDbContext>(cfg =>
        {
            cfg.UseNpgsql(builder.Configuration.GetConnectionString("PostgreSql"));
        });

        builder.Services.AddScoped(sp =>
        {
            var configuration = sp.GetRequiredService<IConfiguration>();
            return new BlobServiceClient(new Uri("https://ll9preunestafiles.blob.core.windows.net/"), new StorageSharedKeyCredential("ll9preunestafiles", configuration["Azure:AccountKey"]));
        });
        builder.Services.AddHttpClient("DownloadClient", cfg => cfg.Timeout = TimeSpan.FromMinutes(5));
        builder.Services.AddScoped<DownloadManager>();
        builder.Services.AddHybridCache(opt =>
        {
            opt.DefaultEntryOptions = new HybridCacheEntryOptions
            {
                Expiration = TimeSpan.FromMinutes(5),
                LocalCacheExpiration = TimeSpan.FromMinutes(5),
            };
        });

        var app = builder.Build();

        app.UseSwagger();
        app.UseSwaggerUI();

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}

public partial class Program { }