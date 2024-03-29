using Microsoft.EntityFrameworkCore;
using ProtoBuf.Grpc.Server;
using CartoonDomain.Service.Services.Commands.v1;
using CartoonDomain.Command.Service.Data;
using CartoonDomain.Service.Data;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.

builder.Services.AddCodeFirstGrpc();
builder.Services.AddCodeFirstGrpcReflection();
builder.Services.AddDbContext<CartoonCommandContext>(options =>
    options.UseSqlite("DataSource=Cartoons.db"));

var app = builder.Build();


//Create database if not exist
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var cartoonCommandContext = services.GetRequiredService<CartoonCommandContext>();
    DbInitializer.Initialize(cartoonCommandContext);
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<CommandService>();
    endpoints.MapCodeFirstGrpcReflectionService();
});

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
