using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CharacterCreateRequest
{
    [ProtoMember(1)]
    public string Name { get; set; }

    [ProtoMember(2)]
    public string? Description { get; set; }

    [ProtoMember(3)]
    public int CartoonId { get; set; }
}
