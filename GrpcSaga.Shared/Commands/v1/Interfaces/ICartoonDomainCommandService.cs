using CartoonDomain.Shared.Commands.v1.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using ProtoBuf.Grpc;

namespace CartoonDomain.Shared.Commands.v1.Interfaces;

[ServiceContract]
public interface ICartoonDomainCommandService
{
    [OperationContract]
    Task<CartoonUpdateResponse?> UpdateCartoonAsync(CartoonUpdateRequest request, CallContext context = default);
}
