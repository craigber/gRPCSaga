using ProtoBuf.Grpc;
using System.ServiceModel;
using StudioDomain.Shared.Queries.v1.Contracts;

namespace StudioDomain.Shared.Queries.v1.Interfaces;

[ServiceContract]
public interface IStudioDomainQueryService
{
    [OperationContract]
    Task<StudioSingleResponse> GetStudioByIdAsync(StudioSingleRequest request, CallContext context = default);

    [OperationContract]
    Task<StudioMultipleResponse> GetAllStudiosAsync(CallContext context = default);
}