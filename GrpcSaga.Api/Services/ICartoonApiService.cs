using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public interface ICartoonApiService
{
    public Task<CartoonViewModel> GetCartoonByIdAsync(int id);
    public Task<IList<CartoonViewModel>> GetAllCartoonsAsync();
    public Task<CartoonViewModel> UpdateCartoonAsync(CartoonUpdateViewModel viewModel);
    public Task<StudioViewModel?> CreateStudioAsync(StudioCreateViewModel viewModel);
}
