using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.Queries.v1.Contracts;

[DataContract]
public class CharacterSingleResponse
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }

    [ProtoMember(3)]
    public string Description { get; set; }

    [ProtoMember(4)]
    public int CartoonId { get; set; }

}
