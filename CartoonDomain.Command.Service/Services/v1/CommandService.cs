using CartoonDomain.Shared.Commands.v1.Contracts;
using CartoonDomain.Shared.Commands.v1.Interfaces;
using ProtoBuf.Grpc;
using Microsoft.EntityFrameworkCore;
using CartoonDomain.Service.Data;
using CartoonDomain.Common.Data.Entities;

namespace CartoonDomain.Service.Services.Commands.v1;

public class CommandService : ICartoonDomainCommandService
{
    // TODO: https://docs.microsoft.com/en-us/dotnet/architecture/grpc-for-wcf-developers/error-handling

    private readonly CartoonCommandContext _context;

    public CommandService(CartoonCommandContext context)
    {
        _context = context;
    }

    public async Task<CartoonDetailsCreateResponse?> CreateCartoonDetailsAsync(CartoonDetailsCreateRequest request, CallContext context = default)
    {
        if (request == null)
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var cartoon = new Cartoon
            {
                Title = request.Cartoon.Title,
                Description = request.Cartoon.Description,
                YearBegin = request.Cartoon.YearBegin,
                YearEnd = request.Cartoon.YearEnd,
                Rating = request.Cartoon.Rating,
                StudioId = request.Cartoon.StudioId
            };
            await _context.Cartoons.AddAsync(cartoon);
            var cartoonSaveCount = _context.SaveChanges();

            if (cartoonSaveCount != 1)
            {
                // Cartoon save failed. Entity Framework will automatically rollback the transaction.
                // However, because the Studio is saved in a different domain and transaction,
                // it must be compensated. This is done in the calling service
                return null;
            }

            // Now that the cartoon has been created, create the characters
            if (request.Cartoon.Characters != null && request.Cartoon.Characters.Any())
            {
                //cartoon.Characters = new List<Character>();
                foreach (var character in request.Cartoon.Characters)
                {
                    var newCharacter = new Character
                    {
                        Name = character.Name,
                        Description = character.Description,
                        CartoonId = cartoon.Id
                    };
                    await _context.Characters.AddAsync(newCharacter);
                    _context.Entry(newCharacter).State = EntityState.Added;
                }
                var characterSaveCount = await _context.SaveChangesAsync();
                if (characterSaveCount != request.Cartoon.Characters.Count)
                {
                    // One or more charactes were not created but because they are all part of the
                    // same transaction, they were all rolled back. We now need to compensate
                    // the create for the Cartoon
                    _context.Remove(cartoon);
                    await _context.SaveChangesAsync();

                    // Now the creation of the Studio needs to be compensated but because
                    // it's a different domain and that transaction has completed, it must
                    // be compensated in the calling service
                    return null;                    
                }
            }

            var characters = _context.Characters.Where(c => c.CartoonId == cartoon.Id);
            var chars = new List<CharacterCreateResponse>();
            foreach(var c in characters)
            {
                chars.Add(new CharacterCreateResponse
                {
                    Id = c.Id,
                    Name = c.Name,
                    Description = c.Description,
                    CartoonId = c.CartoonId
                });
            };
            var response = new CartoonDetailsCreateResponse
            {
                Cartoon = new CartoonCreateResponse
                {
                    Id = cartoon.Id,
                    Title = cartoon.Title,
                    YearBegin = cartoon.YearBegin,
                    YearEnd = cartoon.YearEnd,
                    Rating = cartoon.Rating,
                    Description = cartoon.Description
                },
                Characters = chars
            };
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

    public async Task<CartoonCreateResponse?> CreateCartoonAsync(CartoonCreateRequest request, CallContext context = default)
    {
        if (request == null)
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var cartoon = new Cartoon
            {
                Title = request.Title,
                Description = request.Description,
                YearBegin = request.YearBegin,
                YearEnd = request.YearEnd,
                Rating = request.Rating,
                StudioId = request.StudioId
            };
            _context.Cartoons.Attach(cartoon);
            _context.Entry(cartoon).State = EntityState.Added;
            var createCount = await _context.SaveChangesAsync();
            if (createCount != 1)
            {
                return null;
            }
            var response = new CartoonCreateResponse
            {
                Id = cartoon.Id,
                Title = cartoon.Title,
                YearBegin = cartoon.YearBegin,
                YearEnd = cartoon.YearEnd,
                Description = cartoon.Description,
                Rating = cartoon.Rating,
                StudioId = cartoon.StudioId
            };
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
