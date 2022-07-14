using Cartoonalogue.Api.ViewModels;
using CartoonDomain.Shared.Queries.v1.Contracts;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using CartoonDomain.Shared.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;

namespace Cartoonalogue.Api.Services;

public class CartoonService: ICartoonService
{
    private readonly ILogger<CartoonService> _logger;
    private readonly ICartoonDomainQueryService _cartoonDomainQueryService;
    private readonly IStudioDomainQueryService _studioDomainQueryService;

    public CartoonService(ILogger<CartoonService> logger)
    {
        _logger = logger;

        // TODO: Instantiate the gRPC channel in Program.cs and then inject it here
        var cartoonChannel = GrpcChannel.ForAddress("https://localhost:7227");
        _cartoonDomainQueryService = cartoonChannel.CreateGrpcService<ICartoonDomainQueryService>();

        var studioChannel = GrpcChannel.ForAddress("https://localhost:7129");
        _studioDomainQueryService = studioChannel.CreateGrpcService<IStudioDomainQueryService>();
    }

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var cartoonRequest = new CartoonSingleRequest
            {
                Id = id
            };

            Task[] tasks = new Task[1];
            var cartoonResponse = _cartoonDomainQueryService.GetCartoonByIdAsync(cartoonRequest).Result;
           
            if (cartoonResponse == null)
            {
                // The requested id is not found
                var correlationId = Guid.NewGuid();
                // Write Not Found, requested Id, and correlation Id to log
                return null;
            }

            var viewModelResponse = await MapCartoonAsync(cartoonResponse);

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
                var currentCartoon = await MapCartoonAsync(c);
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

    private async Task<CartoonViewModel> MapCartoonAsync(CartoonSingleResponse response)
    {
        var currentCartoon = new CartoonViewModel
        {
            Id = response.Id,
            Title = response.Title,
            YearBegin = response.YearBegin,
            YearEnd = response.YearEnd,
            Description = response.Description,
            Rating = response.Rating,
            StudioId = response.StudioId,
            StudioName = (await _studioDomainQueryService.GetStudioByIdAsync(new StudioSingleRequest { Id = response.StudioId })).Name
        };
        if (response.Characters.Any())
        {
            currentCartoon.Characters = new List<CharacterViewModel>();
            foreach (var ch in response.Characters)
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
        return currentCartoon;
    }
}
