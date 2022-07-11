using Cartoonalogue.Api.ViewModels;

namespace Cartoonalogue.Api.Services;

public interface ICartoonService
{
    public Task<CartoonViewModel> GetCartoonByIdAsync(int id);
}
