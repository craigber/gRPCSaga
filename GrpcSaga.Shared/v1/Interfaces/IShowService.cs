
using ProtoBuf.Grpc;
using System.ServiceModel;
using ShowDomain.Shared.v1.Contracts;

namespace ShowDomain.Shared.v1.Interfaces;

[ServiceContract]
public interface IShowService
{
    [OperationContract]
    Task<ShowModelReply> GetShow(ShowGetSingleRequest request, CallContext context = default);
}
