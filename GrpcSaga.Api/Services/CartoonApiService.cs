using Cartoonalogue.Api.ViewModels;
using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Contracts;
using ProtoBuf.Grpc.Client;
using CartoonDomain.Shared.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using StudioDomain.Shared.Commands.v1.Interfaces;
using StudioDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using Grpc.Net.Client;

namespace Cartoonalogue.Api.Services;

public class CartoonApiService: ICartoonApiService
{
    private readonly ILogger<CartoonApiService> _logger;
    private readonly ICartoonDomainQueryService _cartoonDomainQueryService;
    private readonly IStudioDomainQueryService _studioDomainQueryService;
    private readonly ICartoonDomainCommandService _cartoonDomainCommandService;
    private readonly IStudioDomainCommandService _studioDomainCommandService;

    public CartoonApiService(ILogger<CartoonApiService> logger)
    {
        _logger = logger;

        // TODO: Instantiate the gRPC channels in Program.cs and then inject them here
        var CartoonQueryChannel = GrpcChannel.ForAddress("https://localhost:7227");
        _cartoonDomainQueryService = CartoonQueryChannel.CreateGrpcService<ICartoonDomainQueryService>();

        var CartoonCommandChannel = GrpcChannel.ForAddress("https://localhost:7006");
        _cartoonDomainCommandService = CartoonCommandChannel.CreateGrpcService<ICartoonDomainCommandService>();

        var studioQueryChannel = GrpcChannel.ForAddress("https://localhost:7129");
        _studioDomainQueryService = studioQueryChannel.CreateGrpcService<IStudioDomainQueryService>();

        var studioCommandChannel = GrpcChannel.ForAddress("https://localhost:7166");
        _studioDomainCommandService = studioCommandChannel.CreateGrpcService<IStudioDomainCommandService>();
    }

    public async Task<CharacterViewModel?> CreateCharacterAsync(CharacterCreateViewModel requestViewModel)
    {
        if (requestViewModel == null || string.IsNullOrEmpty(requestViewModel.Name) || requestViewModel.CartoonId < 1)
        {
            var ex = new ArgumentNullException(nameof(requestViewModel));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var createRequest = new CharacterCreateRequest
            {
                Name = requestViewModel.Name,
                CartoonId = requestViewModel.CartoonId
            };
            var createResponse = await _cartoonDomainCommandService.CreateCharacterAsync(createRequest);
            if (createResponse == null)
            {
                return null;
            }
            var responseViewModel = new CharacterViewModel
            {
                Id = createResponse.Id,
                Name = createResponse.Name,
                CartoonId = createResponse.CartoonId
            };
            return responseViewModel;
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            throw;
        }
    }

    public async Task<StudioViewModel?> CreateStudioAsync(StudioCreateViewModel createViewModel)
    {
        if (createViewModel == null || string.IsNullOrEmpty(createViewModel.Name))
        {
            throw new ArgumentNullException(nameof(createViewModel));
        }

        try
        {
            var createRequest = new StudioCreateRequest
            {
                Name = createViewModel.Name
            };
            var createResponse = await _studioDomainCommandService.CreateStudioAsync(createRequest);
            if (createResponse == null)
            {
                return null;
            }
            var studioViewModel = new StudioViewModel
            {
                Id = createResponse.Id,
                Name = createResponse.Name
            };
            return studioViewModel;
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
        }
    }

     public async Task<CartoonInfoViewModel> UpdateCartoonAsync(CartoonUpdateViewModel updateViewModel)
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
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
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

            var viewModel = new CartoonViewModel
            {
                Id = cartoonResponse.Id,
                Title = cartoonResponse.Title,
                Description = cartoonResponse.Description,
                YearBegin = cartoonResponse.YearBegin,
                YearEnd = cartoonResponse.YearEnd,
                Rating = cartoonResponse.Rating
            };

            return viewModel;
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
        }
    }

    public async Task<CartoonInfoViewModel> GetCartoonInfoByIdAsync(int id)
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
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
        }
    }

    public async Task<IList<CartoonInfoViewModel>> GetAllCartoonInfosAsync()
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

            var cartoons = new List<CartoonInfoViewModel>();
            foreach (var c in getShowResponse.Cartoons)
            {
                var currentCartoon = await MapCartoonAsync(c);
                cartoons.Add(currentCartoon);
            }
            return cartoons;
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
        }
    }

    private async Task<CartoonInfoViewModel> MapCartoonAsync(CartoonSaveRequest response)
    {
        var currentCartoon = new CartoonInfoViewModel
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
