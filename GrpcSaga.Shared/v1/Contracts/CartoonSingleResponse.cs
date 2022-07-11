using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.v1.Contracts;

[ProtoContract]
public class CartoonSingleResponse
{
    [ProtoMember(1)]
    public int Id { get; set; }

    [ProtoMember(2)]
    public string Name { get; set; }

    [ProtoMember(3)]
    public int YearBegin { get; set; }
        
    [ProtoMember(4)]
    public int YearEnd { get; set; }
        
    [ProtoMember(5)]
    public int StudioId { get; set; }
}