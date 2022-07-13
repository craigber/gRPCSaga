using Cartoonalogue.Api.ViewModels;
using CartoonDomain.Shared.Queries.v1.Contracts;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using CartoonDomain.Shared.v1.Interfaces;
using CartoonDomain.Service.Data;

namespace Cartoonalogue.Api.Services;

public class CartoonService: ICartoonService
{
    private readonly ILogger<CartoonService> _logger;
    private readonly ICartoonDomainQueryService _cartoonDomainQueryService;

    public CartoonService(ILogger<CartoonService> logger)
    {
        _logger = logger;

        // TODO: Instantiate the gRPC channel in Program.cs and then inject it here
        var channel = GrpcChannel.ForAddress("https://localhost:7227");
        _cartoonDomainQueryService = channel.CreateGrpcService<ICartoonDomainQueryService>();
    }

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var request = new CartoonSingleRequest
            {
                Id = id
            };

            var getShowResponse = await _cartoonDomainQueryService.GetCartoonByIdAsync(request);

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
                Title = getShowResponse.Title,
                YearBegin = getShowResponse.YearBegin,
                YearEnd = getShowResponse.YearEnd,
                Description = getShowResponse.Description,
                Rating = getShowResponse.Rating,
                StudioId = getShowResponse.StudioId
            };
            return viewModelResponse;
        }
        catch (Exception ex)
        {
            // TODO: catch the RpcException and handle it correctly

            var correlationId = Guid.NewGuid();
            // write exception and correlation Id to log
            throw new Exception($"Id: {correlationId}");
        }
    }

    public async Task<IList<CartoonViewModel>> GetAllCartoonsAsync()
    {
        try
        {
            var getShowResponse = await _cartoonDomainQueryService.GetAllCartoonsAsync();

            if (getShowResponse == null || getShowResponse.Cartoons.Count() == 0)
            {
                // The requested id is not found
                var correlationId = Guid.NewGuid();
                // Write Not Found, requested Id, and correlation Id to log
                return null;
            }

            var cartoons = new List<CartoonViewModel>();
            foreach (var c in getShowResponse.Cartoons)
            {
                var currentCartoon = new CartoonViewModel
                {
                    Id = c.Id,
                    Title = c.Title,
                    YearBegin = c.YearBegin,
                    YearEnd = c.YearEnd,
                    Description = c.Description,
                    Rating = c.Rating,
                    StudioId = c.StudioId
                };
                if (c.Characters.Any())
                {
                    currentCartoon.Characters = new List<CharacterViewModel>();
                    foreach(var ch in c.Characters)
                    {
                        currentCartoon.Characters.Add(new CharacterViewModel
                        {
                            Id = ch.Id,
                            Name = ch.Name,
                            Description = ch.Description,
                            CartoonId = ch.CartoonId
                        });
                    }
                }
                cartoons.Add(currentCartoon);
            }
            return cartoons;
        }
        catch (Exception ex)
        {
            // TODO: catch the RpcException and handle it correctly

            var correlationId = Guid.NewGuid();
            // write exception and correlation Id to log
            throw new Exception($"Id: {correlationId}");
        }
    }
}
