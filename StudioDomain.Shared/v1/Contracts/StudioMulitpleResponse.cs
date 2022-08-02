using ProtoBuf;

namespace StudioDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class StudioMultipleResponse
{
    [ProtoMember(1)]
    public IList<StudioListResponse> Studios { get; set; }
}
