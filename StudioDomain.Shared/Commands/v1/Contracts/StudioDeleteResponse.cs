using ProtoBuf;

namespace StudioDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class StudioDeleteResponse
{
    [ProtoMember(1)]
    public bool Success { get; set; }
}
