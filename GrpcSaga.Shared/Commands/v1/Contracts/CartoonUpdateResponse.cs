using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class CartoonUpdateResponse
{
    [ProtoMember(1)]
    public bool IsSuccess { get; set; }    
}