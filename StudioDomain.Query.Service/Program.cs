using ProtoBuf.Grpc.Server;
using StudioDomain.Service.Services.v1;
using StudioDomain.Service.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddCodeFirstGrpc();
builder.Services.AddCodeFirstGrpcReflection();
builder.Services.AddDbContext<StudioQueryContext>(options =>
    options.UseSqlite("DataSource=Studios.db"));

var app = builder.Build();

//Create database if not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var dbContext = services.GetRequiredService<StudioQueryContext>();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<StudioDomainQueryService>();
    endpoints.MapCodeFirstGrpcReflectionService();
});

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
