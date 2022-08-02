using ProtoBuf;

namespace CartoonDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class CartoonRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }
}
