
using ProtoBuf.Grpc;
using System.ServiceModel;
using CartoonDomain.Shared.Queries.v1.Contracts;

namespace CartoonDomain.Shared.v1.Interfaces;

[ServiceContract]
public interface ICartoonDomainQueryService
{
    [OperationContract]
    Task<CartoonSingleResponse> GetCartoonByIdAsync(CartoonSingleRequest request, CallContext context = default);
}
