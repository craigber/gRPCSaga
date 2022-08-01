using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoBuf;
using System.Runtime.Serialization;

namespace CartoonDomain.Shared.Queries.v1.Contracts;

[ProtoContract]
public class CartoonListResponse
{
    [ProtoMember(1)]
    public IList<CartoonResponse> Cartoons { get; set; }
}
