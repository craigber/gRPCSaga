using ProtoBuf;

namespace StudioDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class StudioRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }
}