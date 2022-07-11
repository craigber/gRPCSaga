
using ProtoBuf.Grpc;
using System.ServiceModel;
using CartoonDomain.Shared.v1.Contracts;

namespace CartoonDomain.Shared.v1.Interfaces;

[ServiceContract]
public interface ICartoonDomainService
{
    [OperationContract]
    Task<CartoonSingleResponse> GetShow(CartoonSingleRequest request, CallContext context = default);
}
