using ProtoBuf;
using System.Runtime.Serialization;

namespace StudioDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class StudioMultipleResponse
{
    [ProtoMember(1)]
    public IList<StudioSingleResponse> Studios { get; set; }
}
