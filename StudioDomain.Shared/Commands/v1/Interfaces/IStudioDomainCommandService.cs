using ProtoBuf.Grpc;
using System.ServiceModel;
using StudioDomain.Shared.Commands.v1.Contracts;

namespace StudioDomain.Shared.Commands.v1.Interfaces;

[ServiceContract]
public interface IStudioDomainCommandService
{
    [OperationContract]
    Task<StudioCreateResponse> CreateStudioAsync(StudioCreateRequest request, CallContext context = default);
}
