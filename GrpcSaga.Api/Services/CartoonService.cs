using Cartoonalogue.Api.ViewModels;
using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Contracts;
using ProtoBuf.Grpc.Client;
using CartoonDomain.Shared.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using Grpc.Net.Client;

namespace Cartoonalogue.Api.Services;

public class CartoonService: ICartoonService
{
    private readonly ILogger<CartoonService> _logger;
    private readonly ICartoonDomainQueryService _cartoonDomainQueryService;
    private readonly IStudioDomainQueryService _studioDomainQueryService;
    private readonly ICartoonDomainCommandService _cartoonDomainCommandService;

    public CartoonService(ILogger<CartoonService> logger)
    {
        _logger = logger;

        // TODO: Instantiate the gRPC channels in Program.cs and then inject them here
        var CartoonQueryChannel = GrpcChannel.ForAddress("https://localhost:7227");
        _cartoonDomainQueryService = CartoonQueryChannel.CreateGrpcService<ICartoonDomainQueryService>();

        var CartoonCommandChannel = GrpcChannel.ForAddress("https://localhost:7006");
        _cartoonDomainCommandService = CartoonCommandChannel.CreateGrpcService<ICartoonDomainCommandService>();

        var studioQueryChannel = GrpcChannel.ForAddress("https://localhost:7129");
        _studioDomainQueryService = studioQueryChannel.CreateGrpcService<IStudioDomainQueryService>();
    }

    public async Task<CartoonViewModel> UpdateCartoonAsync(CartoonUpdateViewModel updateViewModel)
    {
        if (updateViewModel == null || updateViewModel.Id <= 0)
        {
            throw new ArgumentNullException(nameof(updateViewModel));
        }

        try
        {

            var updateRequest = new CartoonUpdateRequest
            {
                Id = updateViewModel.Id,
                Title = updateViewModel.Title,
                YearBegin = updateViewModel.YearBegin,
                YearEnd = updateViewModel.YearEnd,
                Description = updateViewModel.Description,
                Rating = updateViewModel.Rating,
                StudioId = updateViewModel.StudioId
            };
            var updateResponse = await _cartoonDomainCommandService.UpdateCartoonAsync(updateRequest);

            var cartoonRequest = new CartoonSingleRequest
            {
                Id = updateViewModel.Id
            };

            var viewModelResponse = await _cartoonDomainQueryService.GetCartoonByIdAsync(cartoonRequest);
            var cartoonViewModel = await MapCartoonAsync(viewModelResponse);

            return cartoonViewModel;
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // Log exception
            ex.Data.Add("Correlation Id", $"Id: {correlationId}");
            throw ex;
        }

    }

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var cartoonRequest = new CartoonSingleRequest
            {
                Id = id
            };

            //Task[] tasks = new Task[1];
            var cartoonResponse = await _cartoonDomainQueryService.GetCartoonByIdAsync(cartoonRequest);
           
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

    private async Task<CartoonViewModel> MapCartoonAsync(CartoonSaveRequest response)
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
