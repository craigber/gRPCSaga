using CartoonDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using Grpc.Core;
using ProtoBuf.Grpc;
using Microsoft.EntityFrameworkCore;
using CartoonDomain.Service.Data;

namespace CartoonDomain.Service.Services.Commands.v1;

public class CartoonDomainCommandService : ICartoonDomainCommandService
{
    private readonly CartoonCommandContext _context;

    public CartoonDomainCommandService(CartoonCommandContext context)
    {
        _context = context;
    }

    public async Task<CartoonUpdateResponse> UpdateCartoonAsync(CartoonUpdateRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        try
        {
            var cartoon = _context.Cartoons.Find(request.Id);

            if (cartoon == null || request.Id <= 0)
            {
                return null;
            }

            cartoon.Title = request.Title;
            cartoon.YearBegin = request.YearBegin;
            cartoon.YearEnd = request.YearEnd;
            cartoon.Description = request.Description;
            cartoon.Rating = request.Rating;
            cartoon.StudioId = request.StudioId;

            if (cartoon.IsValid())
            {
                await _context.SaveChangesAsync();
                return new CartoonUpdateResponse
                {
                    IsSuccess = true
                };
            }
            return null;
        }
        // You could fold all the catch blocks into a single one, but
        // I wanted to show the specifc Exceptions thrown here
        catch (DbUpdateConcurrencyException ex)
        {
            var correlationId = Guid.NewGuid();
            ex.Data.Add("Correlation Id", $"Id: {correlationId}. ");
            // log exception here
            throw ex;
        }
        catch (DbUpdateException ex)
        {
            var correlationId = Guid.NewGuid();
            ex.Data.Add("Correlation Id", $"Id: {correlationId}. ");
            // log exception here
            throw ex;
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            ex.Data.Add("Correlation Id", $"Id: {correlationId}. ");
            // log exception here
            throw ex;
        }
    }
}
