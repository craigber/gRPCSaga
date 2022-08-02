using ProtoBuf.Grpc.Server;
using CartoonDomain.Query.Service.Services.v1;
using CartoonDomain.Query.Service.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Additional configuration is required to successfully run gRPC on macOS.
// For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682

// Add services to the container.
builder.Services.AddCodeFirstGrpc();
builder.Services.AddDbContext<CartoonQueryContext>(options =>
    options.UseSqlite("DataSource=Cartoons.db"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var cartoonQueryContext = services.GetRequiredService<CartoonQueryContext>();
}

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapGrpcService<CartoonDomainQueryService>();
});

// Configure the HTTP request pipeline.
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
