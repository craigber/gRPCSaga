using Cartoonalogue.Api.ViewModels;


namespace Cartoonalogue.Api.Services;

public class CartoonApiService : ICartoonApiService
{
    private readonly ILogger<CartoonApiService> _logger;
    private readonly IDataService _dataService;


    public CartoonApiService(ILogger<CartoonApiService> logger, IDataService dataService)
    {
        _logger = logger;
        _dataService = dataService;
    }

    public async Task<CartoonDetailsViewModel>? CreateCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel)
    {
        if (createViewModel == null)
        {
            var ex = new ArgumentNullException(nameof(createViewModel));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }
        try
        {
            var viewModel = await _dataService.CreateCartoonDetailsAsync(createViewModel);
            return viewModel;
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

    public async Task<CartoonViewModel>? CreateCartoonAsync(CartoonCreateViewModel createViewModel)
    {
        if (createViewModel == null)
        {
            var ex = new ArgumentNullException(nameof(createViewModel));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }
        try
        {
            var viewModel = await _dataService.CreateCartoonAsync(createViewModel);
            return viewModel;
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
            var viewModel = await _dataService.CreateCharacterAsync(requestViewModel);
            return viewModel;
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
            var viewModel = await _dataService.CreateStudioAsync(createViewModel);
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

    public async Task<CartoonViewModel> UpdateCartoonAsync(CartoonUpdateViewModel updateViewModel)
    {
        if (updateViewModel == null || updateViewModel.Id <= 0)
        {
            throw new ArgumentNullException(nameof(updateViewModel));
        }

        try
        {

            var viewModel = await _dataService.UpdateCartoonAsync(updateViewModel);
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

    public async Task<CartoonViewModel> GetCartoonByIdAsync(int id)
    {
        try
        {
            var viewModel = await _dataService.GetCartoonByIdAsync(id);

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
            var viewModel = await _dataService.GetCartoonDetailsByIdAsync(id);
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

    public async Task<IList<CartoonViewModel>> GetCartoonListAsync()
    {
        try
        {
            var cartoons = await _dataService.GetCartoonListAsync();
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
}
   