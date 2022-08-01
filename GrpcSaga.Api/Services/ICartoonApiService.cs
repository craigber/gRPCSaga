using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public interface ICartoonApiService
{
    public Task<CartoonViewModel>? GetCartoonByIdAsync(int id);
    public Task<CartoonDetailsViewModel>? GetCartoonDetailsByIdAsync(int id);
    public Task<IList<CartoonViewModel>>? GetCartoonListAsync();
    public Task<CartoonViewModel>? CreateCartoonAsync(CartoonCreateViewModel cartoon);
    public Task<CartoonViewModel>? UpdateCartoonAsync(CartoonUpdateViewModel viewModel);
    public Task<StudioViewModel>? CreateStudioAsync(StudioCreateViewModel viewModel);
    public Task<CharacterViewModel>? CreateCharacterAsync(CharacterCreateViewModel viewModel);
    public Task<CartoonDetailsViewModel>? CreateCartoonDetailsAsync(CartoonDetailsCreateViewModel viewModel);
}
