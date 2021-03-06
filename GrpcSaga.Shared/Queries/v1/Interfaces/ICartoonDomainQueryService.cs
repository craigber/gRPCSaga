
using ProtoBuf.Grpc;
using System.ServiceModel;
using CartoonDomain.Shared.Queries.v1.Contracts;

namespace CartoonDomain.Shared.v1.Interfaces;

[ServiceContract]
public interface ICartoonDomainQueryService
{
    [OperationContract]
    Task<CartoonSaveRequest> GetCartoonByIdAsync(CartoonSingleRequest request, CallContext context = default);

    [OperationContract]
    Task<CartoonMultipleResponse> GetAllCartoonsAsync(CallContext context = default);
}
