using ProtoBuf;
namespace StudioDomain.Shared.Commands.v1.Contracts
{
    [ProtoContract]
    public class StudioCreateResponse
    {
        [ProtoMember(1)]
        public int Id { get; set; }

        [ProtoMember(2)]
        
        public string Name { get; set; }
    }
}
