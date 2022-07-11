using GrpcSaga.Api.ViewModels;
using ShowDomain.Shared.v1.Contracts;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ShowDomain.Shared.v1.Interfaces;

namespace GrpcSaga.Api.Services;

public class CartoonService: ICartoonService
{
    private readonly ILogger<CartoonService> _logger;

    public CartoonService(ILogger<CartoonService> logger)
    {
        _logger = logger;
    }
    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var request = new ShowGetSingleRequest
            {
                Id = id
            };

            var channel = GrpcChannel.ForAddress("https://localhost:7227");
            var client = channel.CreateGrpcService<IShowService>();

            var getShowResponse = await client.GetShow(request);

            if (getShowResponse == null)
            {
                // The requested id is not found
                var correlationId = Guid.NewGuid();
                // Write to log
                return null;
            }

            var viewModelResponse = new CartoonViewModel
            {
                Id = getShowResponse.Id,
                Name = getShowResponse.Name,
                YearBegin = getShowResponse.YearBegin,
                YearEnd = getShowResponse.YearEnd,
                StudioId = getShowResponse.StudioId
            };
            return viewModelResponse;
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // write exception and correlation Id to log
            throw new Exception($"Id: {correlationId}");
        }
    }
}
