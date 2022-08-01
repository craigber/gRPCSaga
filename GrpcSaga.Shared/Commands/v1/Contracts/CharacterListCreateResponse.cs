using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CharacterListCreateResponse
{
    [ProtoMember(1)]
    public List<CharacterCreateResponse> Characters { get; set; }
}
