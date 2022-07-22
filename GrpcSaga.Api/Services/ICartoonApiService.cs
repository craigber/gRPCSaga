using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public interface ICartoonApiService
{
    public Task<CartoonViewModel> GetCartoonByIdAsync(int id);
    public Task<CartoonInfoViewModel> GetCartoonInfoByIdAsync(int id);
    public Task<IList<CartoonInfoViewModel>> GetAllCartoonInfosAsync();
    public Task<CartoonInfoViewModel> UpdateCartoonAsync(CartoonUpdateViewModel viewModel);
    public Task<StudioViewModel?> CreateStudioAsync(StudioCreateViewModel viewModel);
    public Task<CharacterViewModel?> CreateCharacterAsync(CharacterCreateViewModel viewModel);
}
