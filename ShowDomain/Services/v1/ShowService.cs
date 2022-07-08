using ProtoBuf.Grpc;
using ShowDomain.Shared.v1.Interfaces;
using ShowDomain.Shared.v1.Contracts;

namespace ShowDomain.Service.Services.v1;

public class ShowService : IShowService
{
    public Task<ShowModelReply> GetShow(ShowGetSingleRequest request, CallContext context = default)
    {
        try
        {
            var reply = new ShowModelReply
            {
                Id = request.Id,
                Name = "Rocky and Friends",
                YearBegin = 1962,
                YearEnd = 1964
            };

            return Task.FromResult(reply);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw new Exception("Rpc exception", ex);
        }
    }
}
