using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class CartoonSingleRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }
}
