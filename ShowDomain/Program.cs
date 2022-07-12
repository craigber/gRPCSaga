using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ProtoBuf.Grpc.Server;
using CartoonDomain.Service.Services.v1;
using CartoonDomain.Service.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddCodeFirstGrpc();
builder.Services.AddDbContext<CartoonQueryContext>(options => options.UseSqlServer("Server=.;Database=Cartoon;Trusted_Connection=True;MultipleActiveResultSets=true"));

var app = builder.Build();

// Create database if not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<CartoonQueryContext>();
    DbInitializer.Initialize(context);
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<CartoonDomainQueryService>();
});

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();


