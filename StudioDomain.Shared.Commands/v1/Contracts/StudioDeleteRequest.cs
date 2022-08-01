using ProtoBuf;

namespace StudioDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class StudioDeleteRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }
}
