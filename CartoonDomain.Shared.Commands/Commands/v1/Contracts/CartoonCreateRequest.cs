using ProtoBuf;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CartoonCreateRequest
{
    [ProtoMember(1)]
    public string Title { get; set; }

    [ProtoMember(2)]
    public int YearBegin { get; set; }

    [ProtoMember(3)]
    public int? YearEnd { get; set; }

    [ProtoMember(4)]
    public string? Description { get; set; }

    [ProtoMember(5)]
    public decimal? Rating { get; set; }

    [ProtoMember(6)]
    public int StudioId { get; set; }

    [ProtoMember(7)]
    public List<CharacterCreateRequest>? Characters { get; set; }
}