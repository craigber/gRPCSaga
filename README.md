# gRPCSaga
A C# sample application demoing gRPC, CQRS, and data sagas.

.NET 6 is required.

Background
I was tasked at my job to create a demo/example/reference application that showed how to use both gRPC and transactional consistancy across databases/domains.

This sample application is a catalog of cartoons -- a Cartoonalogue.

## Building and Running the Application
1. Once you have the source on disk, run Visual Studio and open the GprcSaga solution
2. Right-click on Solution 'GrpcSaga' (this is the top most item) in the Solution Explorer. 
3. Select Properties (this is the bottom-most item) in the pop-menu.
4. Select Multiple startup projects
5. Set the following projects to Start: Cartoonalogue.Api, CartoonDomain.Command.Service, CartoonDomain.Query.Service, StudioDomain.Command.Service, StudioDomain.Query.Service
6. Click OK
7. Build and run the solution

## Included VS Projects
The sample is split into several projects:
- Cartoonalogue
  - Cartoonalogue.Api: An ASP.Net Core Api project that provides a UI via a Swagger page. It makes REST calls from the browser to controllers. When you launch the solution inside Visual Studio, a Swagger page is presented where you can Try Out the API.
- CartoonDomain
  - CartoonDomain.Query.Service: A gRPC server for CQRS queries into the Cartoon Domain
  - CartoonDomain.Command.Service: A gRPC server for CQRS commands into the Cartoon Domain
  - CartoonDomain.Common: A class library of common code used by the Cartoon Domain server projects
  - CartoonDomain.Shared: A public class library to be shared with client projects. In a real application, this project would most likely be a Cartoon Domain Nuget package
- Studio Domain
  - StudioDomain.Query.Service: A gRPC server for CQRS queries into the Studio Domain
  - StudioDomain.Command.Service: A gRPC server for CQRS commands into the Studio Domain
  - StudioDomain.Common: A class library of common code used the Studio Domain server projects
  - StudioDomain.Shared: A public class library to be shared with client projects. In a real application, this project would most likely be a Studio Domain Nuget package

## gRPC Information
Most of the gRPC examples you see use .proto files that are ingested at compile time and contract files are spit out by the .Net compiler. This example is built with a code-first pattern. There are no .proto files.

Useful links:
- [Offical Microsoft gRPC docs](https://docs.microsoft.com/en-us/aspnet/core/tutorials/grpc/grpc-start?view=aspnetcore-6.0&tabs=visual-studio)
- [gRPC-dotnet examples](https://github.com/grpc/grpc-dotnet/tree/master/examplesg). Check out the Coder example for code-first.
- [protobuf-net.Grpc](https://protobuf-net.github.io/protobuf-net.Grpc/gettingstarted). This library makes code-first possible.

## Data access
SQLite is used to make this application portable. The CQRS pattern is used to separate queries and commands. Eventually, the Saga Pattern will be used to enforce transactions to multiple database/domains.

The idea of entities representing the schema of a table is strictly enforced. An entity object does not exist outside of the domain layer...that's the job of a model. View models are exclusively used for the UI layer.

## Design ideas and tradeoffs
- There are libraries that support gRPC in a web frontend, but they are not officially supported by gRPC so the API is setup as a RESTful web service.
- .proto files were not used because I was tasked with going code-first. I personally like this option but it means non-.Net clients won't work without additional effort.
- It may seem there is lots of data mapping, but this gives more flexability. For example, using request and response objects means the data sent can be easily changed, espcially with the gRPC services.