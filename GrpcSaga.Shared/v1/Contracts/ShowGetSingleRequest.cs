using ProtoBuf;
using System.Runtime.Serialization;

namespace ShowDomain.Shared.v1.Contracts;

[ProtoContract]
public class ShowGetSingleRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }
}
