using Grpc.Core;
using ProtoBuf.Grpc;
using CartoonDomain.Shared.v1.Interfaces;
using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Query.Service.Data;
using CartoonDomain.Common.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Query.Service.Services.v1;

public class CartoonDomainQueryService : ICartoonDomainQueryService
{
    private readonly CartoonQueryContext _context;

    public CartoonDomainQueryService(CartoonQueryContext context)
    {
        _context = context;
    }

    public async Task<CartoonResponse> GetCartoonByIdAsync(CartoonRequest request, CallContext context = default)
    {
        if (request.Id <= 0)
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var cartoon = await _context.Cartoons.Include("Characters").SingleOrDefaultAsync(c => c.Id == request.Id);

            if (cartoon == null)
            {
                return null;
            }

            var response = MapToSingleResponse(cartoon);
            return response;

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

    public async Task<CartoonListResponse> GetAllCartoonsAsync(CallContext context = default)
    {
        try
        {
            var cartoons = _context.Cartoons.Include("Characters").ToList();

            if (cartoons == null || cartoons.Count == 0)
            {
                return null;
            }

            var response = new CartoonListResponse();
            response.Cartoons = new List<CartoonResponse>();
            foreach (var c in cartoons)
            {
                response.Cartoons.Add(MapToSingleResponse(c));
            }
            return response;
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

    private CartoonResponse MapToSingleResponse(Cartoon cartoon)
    {
        var response = new CartoonResponse
        {
            Id = cartoon.Id,
            Title = cartoon.Title,
            YearBegin = cartoon.YearBegin,
            YearEnd = cartoon.YearEnd,
            Description = cartoon.Description,
            Rating = cartoon.Rating,
            StudioId = cartoon.StudioId        
        };

        if (cartoon.Characters != null && cartoon.Characters.Any())
        {
            response.Characters = new List<CharacterResponse>();
            foreach (var character in cartoon.Characters)
            {
                response.Characters.Add(new CharacterResponse
                {
                    Id = character.Id,
                    Name = character.Name,
                    Description = character.Description,
                    CartoonId = character.CartoonId
                });
            }
        }
        return response;
    }
}
