using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public interface IDataService
{
    public Task<StudioViewModel> GetStudioByIdAsync(int id);
    public Task<CartoonViewModel> CreateCartoonAsync(CartoonCreateViewModel createViewModel);

    public Task<CharacterViewModel> CreateCharacterAsync(CharacterCreateViewModel createViewModel);

    public Task<StudioViewModel> CreateStudioAsync(StudioCreateViewModel createViewModel);

    public Task<CartoonViewModel> UpdateCartoonAsync(CartoonUpdateViewModel updateViewModel);

    public Task<CartoonViewModel> GetCartoonByIdAsync(int id);

    public Task<CartoonDetailsViewModel> GetCartoonDetailsByIdAsync(int id);

    public Task<IList<CartoonViewModel>> GetCartoonListAsync();

    public Task<CartoonDetailsViewModel> CreateCartoonDetailsAsync(CartoonDetailsCreateViewModel createViewModel);
}
