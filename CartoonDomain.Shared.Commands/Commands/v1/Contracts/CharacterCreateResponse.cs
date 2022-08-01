using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CharacterCreateResponse
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }

    [ProtoMember(3)]
    public string? Description { get; set; }

    [ProtoMember(4)]
    public int CartoonId { get; set; }  
}
