using GrpcSaga.Api.ViewModels;

namespace GrpcSaga.Api.Services;

public interface ICartoonService
{
    public Task<CartoonViewModel> GetCartoonByIdAsync(int id);
}
