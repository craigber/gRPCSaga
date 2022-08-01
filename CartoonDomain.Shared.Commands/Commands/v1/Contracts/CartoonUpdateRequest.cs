using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CartoonUpdateRequest
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Title { get; set; }

    [ProtoMember(3)]
    public int YearBegin { get; set; }

    [ProtoMember(4)]
    public int? YearEnd { get; set; }

    [ProtoMember(5)]
    public string? Description { get; set; }

    [ProtoMember(6)]
    public decimal? Rating { get; set; }

    [ProtoMember(7)]
    public int StudioId { get; set; }
}