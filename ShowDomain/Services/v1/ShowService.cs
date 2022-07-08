﻿using Grpc.Core;
using ProtoBuf.Grpc;
using ShowDomain.Shared.v1.Interfaces;
using ShowDomain.Shared.v1.Contracts;

namespace ShowDomain.Service.Services.v1;

public class ShowService : IShowService
{
    public Task<ShowModelReply> GetShow(ShowGetSingleRequest request, CallContext context = default)
    {
        if (request.Id <= 0 || request.Id > 2)
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid Id"));
        }

        try
        {
            var shows = new List<ShowModelReply>();
            shows.Add(new ShowModelReply
            {
                Id = 1,
                Name = "Rocky and Friends",
                YearBegin = 1959,
                YearEnd = 1963
            });

            shows.Add(new ShowModelReply
            {
                Id = 2,
                Name = "The Simpsons",
                YearBegin = 1989
            });


            return Task.FromResult(shows[request.Id - 1]);
        }
        catch (Exception ex)
        {
            throw new RpcException(new Status(StatusCode.Unknown, ex.Message));
        }
    }
}
