using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CharacterListCreateRequest
{
    [ProtoMember(1)]
    public IList<CharacterCreateRequest> Characters { get; set; }
}
