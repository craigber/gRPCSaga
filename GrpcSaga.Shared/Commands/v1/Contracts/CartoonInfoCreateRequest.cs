using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CartoonDetailsCreateRequest
{
    [ProtoMember(1)]
    public CartoonCreateRequest Cartoon { get; set; }

    [ProtoMember(2)]
    public List<CharacterCreateRequest?> Characters { get; set; }
}
