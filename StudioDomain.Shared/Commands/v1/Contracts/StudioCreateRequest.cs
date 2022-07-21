﻿using ProtoBuf;
using System.Runtime.Serialization;

namespace StudioDomain.Shared.Commands.v1.Contracts;

[ProtoContract]
public class StudioCreateRequest
{
    [ProtoMember(1)]
    public string Name { get; set; }
}
