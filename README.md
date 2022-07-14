# gRPCSaga
A C# sample application for gRPC and data sagas.

.NET 6 is required.

Background
I was tasked at my job to create a demo/example/reference application that showed how to use both gRPC and transactional consistancy across databases/domains.

This sample application is a catalog of cartoons -- a Cartoonalogue.

There are three projects:
- Cartoonalogue.Api: This is an ASP.Net Core Api project that provides a UI via a Swagger page. It makes REST calls from the browser to controllers. When you launch the solution inside Visual Studio, a Swagger page is presented where you can Try Out the API.
- CartoonDomain.Service: The business and data logic for the cartoon domain. This is called from the Cartoonalogue.Api controller via gRPC.
- CartoonDomain.Shared: Interfaces and contracts needed by both the above projects. In a product app, you would probably make this available as a Nuget package.

## gRPC Information
Most of the gRPC examples you see use .proto files that are ingested at compile time and contract files are spit out by the .Net compiler. The gRPCSaga example is built with a code-first pattern. There are no .proto files.

Useful links:
- [Offical Microsoft gRPC docs](https://docs.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-6.0&tabs=visual-studio)
- [gRPC-dotnet examples](https://github.com/grpc/grpc-dotnet/tree/master/examplesg). Check out the Coder example for code-first.
- [protobuf-net.Grpc](https://protobuf-net.github.io/protobuf-net.Grpc/gettingstarted). This library makes code-first possible.

## Data access
SQLite is used to make this application portable. The CQRS pattern is used to separate queries and commands. Eventually, the Saga Pattern will be used to enforce transactions to multiple database/domains.

## Design tradeoffs
- There are libraries that support gRPC in a web frontend, but they are not officially supported by gRPC.
- .proto files were not used because I was tasked with going code-first. I personally like this option but it means non-.Net clients won't work without additional effort.