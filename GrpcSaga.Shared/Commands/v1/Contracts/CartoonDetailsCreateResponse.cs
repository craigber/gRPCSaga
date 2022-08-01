using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CartoonDetailsCreateResponse
{
    [ProtoMember(1)]
    public CartoonCreateResponse Cartoon { get; set; }

    [ProtoMember(2)]
    public IList<CharacterCreateResponse?> Characters { get; set; }
}
