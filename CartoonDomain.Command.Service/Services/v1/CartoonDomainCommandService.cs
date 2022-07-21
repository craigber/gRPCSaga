using CartoonDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using ProtoBuf.Grpc;
using Microsoft.EntityFrameworkCore;
using CartoonDomain.Service.Data;
using CartoonDomain.Common.Data.Entities;

namespace CartoonDomain.Service.Services.Commands.v1;

public class CartoonDomainCommandService : ICartoonDomainCommandService
{
    private readonly CartoonCommandContext _context;

    public CartoonDomainCommandService(CartoonCommandContext context)
    {
        _context = context;
    }

    public async Task<CharacterCreateResponse?> CreateCharacterAsync(CharacterCreateRequest request, CallContext context = default)
    {
        if (request == null || string.IsNullOrEmpty(request.Name))
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var character = new Character
            {
                Name = request.Name,
                Description = request.Description,
                CartoonId = request.CartoonId,
            };
            _context.Characters.Attach(character);
            _context.Entry(character).State = EntityState.Added;
            var updateCount = await _context.SaveChangesAsync();
            if (updateCount > 0)
            {
                var response = new CharacterCreateResponse
                {
                    Id = character.Id,
                    Name = character.Name,
                    Description = character.Description,
                    CartoonId = character.CartoonId
                };
                return response;
            }
            return null;
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

    public async Task<CartoonUpdateResponse?> UpdateCartoonAsync(CartoonUpdateRequest request, CallContext context = default)
    {
        if (request == null)
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
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
                _context.Cartoons.Attach(cartoon);
                _context.Entry(cartoon).State = EntityState.Modified;
                var updateCount = await _context.SaveChangesAsync();
                return new CartoonUpdateResponse
                {
                    IsSuccess = updateCount > 0                
                };
                
            }
            return null;
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
