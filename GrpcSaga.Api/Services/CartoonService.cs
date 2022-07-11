using Cartoonalogue.Api.ViewModels;
using CartoonDomain.Shared.v1.Contracts;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using CartoonDomain.Shared.v1.Interfaces;

namespace Cartoonalogue.Api.Services;

public class CartoonService: ICartoonService
{
    private readonly ILogger<CartoonService> _logger;
    private readonly ICartoonDomainService _showService;

    public CartoonService(ILogger<CartoonService> logger)
    {
        _logger = logger;
        var channel = GrpcChannel.ForAddress("https://localhost:7227");
        _showService = channel.CreateGrpcService<ICartoonDomainService>();
    }

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var request = new CartoonSingleRequest
            {
                Id = id
            };
                       
            var getShowResponse = await _showService.GetShow(request);

            if (getShowResponse == null)
            {
                // The requested id is not found
                var correlationId = Guid.NewGuid();
                // Write Not Found, requested Id, and correlation Id to log
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
