using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using StudioDomain.Shared.Commands.v1.Interfaces;
using StudioDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public class DataService : IDataService
{
    private readonly ICartoonDomainQueryService _cartoonDomainQueryService;
    private readonly IStudioDomainQueryService _studioDomainQueryService;
    private readonly ICartoonDomainCommandService _cartoonDomainCommandService;
    private readonly IStudioDomainCommandService _studioDomainCommandService;

    public DataService(
        ICartoonDomainQueryService? cartoonDomainQueryService = null,
        ICartoonDomainCommandService? cartoonDomainCommandService = null,
        IStudioDomainQueryService? studioDomainQueryService = null,
        IStudioDomainCommandService? studioDomainCommandService = null)
    {
        // This is a kludge to handle these services but I have not yet figured out how to
        // instantiate the gRPC services in program.cs and inject them here. By instantiating 
        // here, we are able to inject mock services for unit testing
        if (cartoonDomainQueryService == null)
        {
            var cartoonQueryChannel = GrpcChannel.ForAddress("https://localhost:7227");
            _cartoonDomainQueryService = cartoonQueryChannel.CreateGrpcService<ICartoonDomainQueryService>();
        }
        else
        {
            _cartoonDomainQueryService = cartoonDomainQueryService;
        }

        if (cartoonDomainCommandService == null)
        {
            var cartoonCommandChannel = GrpcChannel.ForAddress("https://localhost:7006");
            _cartoonDomainCommandService = cartoonCommandChannel.CreateGrpcService<ICartoonDomainCommandService>();
        }
        else
        {
            _cartoonDomainCommandService = cartoonDomainCommandService;
        }

        if (studioDomainQueryService == null)
        {
            var studioQueryChannel = GrpcChannel.ForAddress("https://localhost:7129");
            _studioDomainQueryService = studioQueryChannel.CreateGrpcService<IStudioDomainQueryService>();
        }
        else
        {
            _studioDomainQueryService = studioDomainQueryService;
        }

        if (studioDomainCommandService == null)
        {
            var studioCommandChannel = GrpcChannel.ForAddress("https://localhost:7166");
            _studioDomainCommandService = studioCommandChannel.CreateGrpcService<IStudioDomainCommandService>();
        }
        else
        {
            _studioDomainCommandService= studioDomainCommandService;
        }
    }

    public async Task<StudioViewModel> GetStudioByIdAsync(int id)
    {
        var studioRequest = new StudioRequest
        {
            Id = id
        };
        var studioResponse = await _studioDomainQueryService.GetStudioByIdAsync(studioRequest);
        var viewModel = new StudioViewModel
        {
            Id = studioResponse.Id,
            Name = studioResponse.Name
        };
        return viewModel;
    }

    public async Task<CartoonViewModel> CreateCartoonAsync(CartoonCreateViewModel createViewModel)
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
        var viewModel = new CartoonViewModel
        {
            Id = createResponse.Id,
            Title = createResponse.Title,
            YearBegin = createResponse.YearBegin,
            YearEnd = createResponse.YearEnd,
            Rating = createResponse.Rating,
            Description = createResponse.Description,
            StudioId = createResponse.StudioId
        };
        return viewModel;
    }

    public async Task<CharacterViewModel> CreateCharacterAsync(CharacterCreateViewModel createViewModel)
    {
        var createRequest = new CharacterCreateRequest
        {
            Name = createViewModel.Name,
            Description = createViewModel.Description,
            CartoonId = createViewModel.CartoonId
        };
        var createResponse = await _cartoonDomainCommandService.CreateCharacterAsync(createRequest);
        if (createResponse == null)
        {
            return null;
        }
        var viewModel = new CharacterViewModel
        {
            Id = createResponse.Id,
            Name = createResponse.Name,
            Description = createResponse.Description,
            CartoonId = createResponse.CartoonId
        };
        return viewModel;
    }

    public async Task<StudioViewModel> CreateStudioAsync(StudioCreateViewModel createViewModel)
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
        var viewModel = new StudioViewModel
        {
            Id = createResponse.Id,
            Name = createResponse.Name
        };
        return viewModel;
    }

    public async Task<CartoonViewModel> UpdateCartoonAsync(CartoonUpdateViewModel updateViewModel)
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

        var response = await _cartoonDomainQueryService.GetCartoonByIdAsync(cartoonRequest);
        var viewModel = new CartoonViewModel
        {
            Id = response.Id,
            Title = response.Title,
            YearBegin = response.YearBegin,
            YearEnd = response.YearEnd,
            Description = response.Description,
            Rating = response.Rating,
            StudioId = response.StudioId
        };
        return viewModel;
    }

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
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

    public async Task<CartoonDetailsViewModel> GetCartoonDetailsByIdAsync(int id)
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

        var veiwModel = new CartoonDetailsViewModel
        {
            Cartoon = new CartoonViewModel
            {
                Id = cartoonResponse.Id,
                Title = cartoonResponse.Title,
                YearBegin = cartoonResponse.YearBegin,
                YearEnd = cartoonResponse.YearEnd,
                Description = cartoonResponse.Description,
                Rating = cartoonResponse.Rating,
                StudioId = cartoonResponse.StudioId
            },
            Studio = new StudioViewModel
            {
                Id = cartoonResponse.StudioId,
                Name = (await _studioDomainQueryService.GetStudioByIdAsync(new StudioRequest { Id = cartoonResponse.StudioId })).Name
            }
        };
        if (cartoonResponse.Characters.Any())
        {
            veiwModel.Cartoon.Characters = new List<CharacterViewModel>();
            foreach (var c in cartoonResponse.Characters)
            {
                veiwModel.Cartoon.Characters.Add(new CharacterViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CartoonId = c.CartoonId
                });
            }
        }
        return veiwModel;
    }

    public async Task<IList<CartoonViewModel>> GetCartoonListAsync()
    {
        var cartoonResponse = await _cartoonDomainQueryService.GetAllCartoonsAsync();

        if (cartoonResponse == null || cartoonResponse.Cartoons.Count() == 0)
        {
            // The requested id is not found
            var correlationId = Guid.NewGuid();
            // Write Not Found, requested Id, and correlation Id to log
            return null;
        }

        var cartoons = new List<CartoonViewModel>();
        foreach (var c in cartoonResponse.Cartoons)
        {
            var currentCartoon = new CartoonViewModel
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                YearBegin = c.YearBegin,
                YearEnd = c.YearEnd,
                Rating = c.Rating,
                StudioId = c.StudioId
            };
            cartoons.Add(currentCartoon);
        }
        return cartoons;
    }

    public async Task<CartoonDetailsViewModel> CreateCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel)
    {
        var isNewStudio = false;

        // 1. Either insert or retrieve the studio
        StudioViewModel? studioViewModel;
        if (createViewModel.Cartoon.StudioId <= 0)
        {
            // Add new Studio
            studioViewModel = await CreateStudioAsync(createViewModel.Studio);
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
            studioViewModel = await GetStudioByIdAsync(createViewModel.Cartoon.StudioId);
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

        var viewModel = new CartoonDetailsViewModel
        {
            Cartoon = cartoonViewModel,
            Studio = studioViewModel
        };
        return viewModel;
    }

    private async Task<CartoonViewModel?> InsertCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel, int studioId)
    {
        var charactersCreateRequest = new List<CharacterCreateRequest>();
        if (createViewModel.Characters != null && createViewModel.Characters.Any())
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
        var cartoonRequest = new CartoonDetailsCreateRequest
        {
            Cartoon = new CartoonCreateRequest
            {
                Title = createViewModel.Cartoon.Title,
                Description = createViewModel.Cartoon.Description,
                YearBegin = createViewModel.Cartoon.YearBegin,
                YearEnd = createViewModel.Cartoon.YearEnd,
                Rating = createViewModel.Cartoon.Rating,
                StudioId = studioId,
                Characters = charactersCreateRequest
            }
        };

        var cartoonResponse = await _cartoonDomainCommandService.CreateCartoonDetailsAsync(cartoonRequest);
        if (cartoonResponse == null)
        {
            // Cartoon insert failed
            return null;
        }

        var charactersResponse = new List<CharacterViewModel>();
        if (cartoonResponse.Cartoon.Characters != null && cartoonResponse.Cartoon.Characters.Count > 0)
        {
            foreach (var c in cartoonResponse.Cartoon.Characters)
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
            Id = cartoonResponse.Cartoon.Id,
            Title = cartoonResponse.Cartoon.Title,
            Description = cartoonResponse.Cartoon.Description,
            YearBegin = cartoonResponse.Cartoon.YearBegin,
            YearEnd = cartoonResponse.Cartoon.YearEnd,
            Rating = cartoonResponse.Cartoon.Rating,
            StudioId = cartoonResponse.Cartoon.StudioId,
            Characters = charactersResponse
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
}
