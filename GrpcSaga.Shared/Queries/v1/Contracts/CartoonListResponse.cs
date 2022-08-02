using ProtoBuf;

namespace CartoonDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class CartoonListResponse
{
    [ProtoMember(1)]
    public IList<CartoonResponse> Cartoons { get; set; }
}
