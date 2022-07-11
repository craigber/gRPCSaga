using Grpc.Core;
using ProtoBuf.Grpc;
using CartoonDomain.Shared.v1.Interfaces;
using CartoonDomain.Shared.v1.Contracts;

namespace CartoonDomain.Service.Services.v1;

public class CartoonDomainService : ICartoonDomainService
{
    public Task<CartoonSingleResponse> GetShow(CartoonSingleRequest request, CallContext context = default)
    {
        if (request.Id <= 0 || request.Id > 2)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.NotFound, $"Id: {correlationId}"));
        }

        try
        {
            var shows = new List<CartoonSingleResponse>();
            shows.Add(new CartoonSingleResponse
            {
                Id = 1,
                Name = "Rocky and Friends",
                YearBegin = 1959,
                YearEnd = 1963
            });

            shows.Add(new CartoonSingleResponse
            {
                Id = 2,
                Name = "The Simpsons",
                YearBegin = 1989
            });


            return Task.FromResult(shows[request.Id - 1]);
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.Unknown, $"Id: {correlationId}"));
        }
    }
}
