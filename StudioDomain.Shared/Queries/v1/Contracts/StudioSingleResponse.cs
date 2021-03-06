using ProtoBuf;
using System.Runtime.Serialization;

namespace StudioDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class StudioSingleResponse
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }
}
