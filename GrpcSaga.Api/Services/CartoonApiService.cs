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

public class CartoonApiService : ICartoonApiService
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

    private async Task<StudioViewModel?> InsertStudioAsync(StudioCreateViewModel studio)
    {
        var studioRequest = new StudioCreateRequest
        {
            Name = studio.Name
        };
        var studioResponse = await _studioDomainCommandService.CreateStudioAsync(studioRequest);
        if (studioResponse == null)
        {
            // Studio insert failed
            return null;
        }
        var response = new StudioViewModel
        {
            Id = studioResponse.Id,
            Name = studioResponse.Name
        };
        return response;
    }

    private async Task<bool> CompensateStudioAsync(StudioViewModel studio)
    {
        var studioRequest = new StudioDeleteRequest
        {
            Id = studio.Id
        };
        var studioResponse = await _studioDomainCommandService.DeleteStudioAsync(studioRequest);
        if (studioResponse == null)
        {
            // Log the delete failure
        }
        return studioResponse != null;
    }

    private async Task<CartoonViewModel?> InsertCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel, int studioId)
    {
        var charactersCreateRequest = new List<CharacterCreateRequest>();
        if (createViewModel.Characters != null && createViewModel.Characters.Count > 0)
        {
            foreach (var c in createViewModel.Characters)
            {
                charactersCreateRequest.Add(new CharacterCreateRequest
                {
                    Name = c.Name,
                    Description = c.Description
                });
            }
        }
        var cartoonRequest = new CartoonCreateRequest
        {
            Title = createViewModel.Cartoon.Title,
            Description = createViewModel.Cartoon.Description,
            YearBegin = createViewModel.Cartoon.YearBegin,
            YearEnd = createViewModel.Cartoon.YearEnd,
            Rating = createViewModel.Cartoon.Rating,
            StudioId = studioId,
            Characters = charactersCreateRequest
        };
        var cartoonResponse = await _cartoonDomainCommandService.CreateCartoonAsync(cartoonRequest);
        if (cartoonResponse == null)
        {
            // Cartoon insert failed
            return null;
        }

        var charactersResponse = new List<CharacterViewModel>();
        if (cartoonResponse.Characters != null && cartoonResponse.Characters.Count > 0)
        {
            foreach (var c in cartoonResponse.Characters)
            {
                charactersResponse.Add(new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CartoonId = c.CartoonId
                });
            }
        }
        var response = new CartoonViewModel
        {
            Id = cartoonResponse.Id,
            Title = cartoonResponse.Title,
            Description = cartoonResponse.Description,
            YearBegin = cartoonResponse.YearBegin,
            YearEnd = cartoonResponse.YearEnd,
            Rating = cartoonResponse.Rating,
            StudioId = cartoonResponse.StudioId
        };
        return response;
    }


    /////////////////////////////////////////////////////////////https://docs.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/error-handling

    private async Task<List<CharacterViewModel?>> InsertCharacterListAsync(List<CharacterCreateViewModel> characters, CartoonViewModel cartoon)
    {
        return null;
    }

    public async Task<CartoonDetailsViewModel?> CreateCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel)
    {
        if (createViewModel == null)
        {
            var ex = new ArgumentNullException(nameof(createViewModel));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }
        try
        {
            var isNewStudio = false;

            // 1. Either insert or retrieve the studio
            StudioViewModel? studioViewModel;
            if (createViewModel.Cartoon.StudioId <= 0)
            {
                // Add new Studio
                studioViewModel = await InsertStudioAsync(createViewModel.Studio);
                if (studioViewModel == null)
                {
                    // Studio database update failed
                    return null;
                }
                isNewStudio = true;
            }
            else
            {
                // Studio already exists so retrieve it
                var studioRequest = new StudioRequest
                {
                    Id = createViewModel.Cartoon.StudioId
                };
                var studioResponse = await _studioDomainQueryService.GetStudioByIdAsync(studioRequest);
                studioViewModel = new StudioViewModel
                {
                    Id = studioResponse.Id,
                    Name = studioResponse.Name
                };
            }

            // 2. Insert Cartoon
            var cartoonViewModel = await InsertCartoonDetailsAsync(createViewModel, studioViewModel.Id);
            if (cartoonViewModel == null)
            {
                // Cartoon database update failed
                if (isNewStudio)
                {
                    await CompensateStudioAsync(studioViewModel);
                }
                return null;
            }

            var response = new CartoonDetailsViewModel
            {
                Cartoon = cartoonViewModel,
                Studio = studioViewModel
            };
            return response;
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

    private async Task DeleteStudioAsync(int id)
    {
        var request = new StudioDeleteRequest
        {
            Id = id
        };
        var response = await _studioDomainCommandService.DeleteStudioAsync(request);
    }

    public async Task<CartoonViewModel?> CreateCartoonAsync(CartoonCreateViewModel createViewModel)
    {
        if (createViewModel == null)
        {
            var ex = new ArgumentNullException(nameof(createViewModel));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }
        try
        {
            var createRequest = new CartoonCreateRequest
            {
                Title = createViewModel.Title,
                YearBegin = createViewModel.YearBegin,
                YearEnd = createViewModel.YearEnd,
                Rating = createViewModel.Rating,
                Description = createViewModel.Description,
                StudioId = createViewModel.StudioId
            };
            var createResponse = await _cartoonDomainCommandService.CreateCartoonAsync(createRequest);
            if (createResponse == null)
            {
                return null;
            }
            var responseViewModel = new CartoonViewModel
            {
                Id = createResponse.Id,
                Title = createResponse.Title,
                YearBegin = createResponse.YearBegin,
                YearEnd = createResponse.YearEnd,
                Rating = createResponse.Rating,
                Description = createResponse.Description,
                StudioId = createResponse.StudioId
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
                Description = requestViewModel.Description,
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
                Description = createResponse.Description,
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

            var cartoonRequest = new CartoonRequest
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
        // TODO: Change this to not return characters
        try
        {
            var cartoonRequest = new CartoonRequest
            {
                Id = id
            };

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
                Rating = cartoonResponse.Rating,
                StudioId = cartoonResponse.StudioId
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

    public async Task<CartoonDetailsViewModel> GetCartoonDetailsByIdAsync(int id)
    {
        try
        {
            var cartoonRequest = new CartoonRequest
            {
                Id = id
            };

            var cartoonResponse = await _cartoonDomainQueryService.GetCartoonByIdAsync(cartoonRequest);

            if (cartoonResponse == null)
            {
                // The requested id is not found
                var correlationId = Guid.NewGuid();
                // Write Not Found, requested Id, and correlation Id to log
                return null;
            }

            var viewModelResponse = await MapCartoonDetailsAsync(cartoonResponse);

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

    public async Task<IList<CartoonViewModel>> GetCartoonListAsync()
    {
        // TODO: Change this to just cartoon, not info
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
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Write to log
            throw;
        }
    }

    private async Task<CartoonViewModel> MapCartoonAsync(CartoonResponse response)
    {
        var currentCartoon = new CartoonViewModel
        {
            Id = response.Id,
            Title = response.Title,
            YearBegin = response.YearBegin,
            YearEnd = response.YearEnd,
            Description = response.Description,
            Rating = response.Rating,
            StudioId = response.StudioId
        };
        return currentCartoon;
    }

    private async Task<CartoonDetailsViewModel> MapCartoonDetailsAsync(CartoonResponse response)
    {
        var cartoonDetails = new CartoonDetailsViewModel
        {
            Cartoon = new CartoonViewModel
            {
                Id = response.Id,
                Title = response.Title,
                YearBegin = response.YearBegin,
                YearEnd = response.YearEnd,
                Description = response.Description,
                Rating = response.Rating,
                StudioId= response.StudioId
            },
            Studio = new StudioViewModel
            {
                Id = response.StudioId,
                Name = (await _studioDomainQueryService.GetStudioByIdAsync(new StudioRequest { Id = response.StudioId })).Name
            }
        };
        if (response.Characters.Any())
        {
            cartoonDetails.Characters = new List<CharacterViewModel>();
            foreach (var c in response.Characters)
            {
                cartoonDetails.Characters.Add(new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CartoonId = c.CartoonId
                });
            }
        }
        return cartoonDetails;
    }
}
