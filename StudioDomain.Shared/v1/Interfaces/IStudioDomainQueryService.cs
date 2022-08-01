using ProtoBuf.Grpc;
using System.ServiceModel;
using StudioDomain.Shared.Queries.v1.Contracts;

namespace StudioDomain.Shared.Queries.v1.Interfaces;

[ServiceContract]
public interface IStudioDomainQueryService
{
    [OperationContract]
    Task<StudioListResponse> GetStudioByIdAsync(StudioRequest request, CallContext context = default);

    [OperationContract]
    Task<StudioMultipleResponse> GetAllStudiosAsync(CallContext context = default);
}