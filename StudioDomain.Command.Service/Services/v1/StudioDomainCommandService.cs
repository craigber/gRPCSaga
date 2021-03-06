using ProtoBuf.Grpc;
using Grpc.Core;
using StudioDomain.Command.Service.Data;
using StudioDomain.Common.Data.Entities;
using StudioDomain.Shared.Commands.v1.Contracts;
using StudioDomain.Shared.Commands.v1.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudioDomain.Command.Service.Services.v1;

public class StudioDomainCommandService : IStudioDomainCommandService
{
    private readonly StudioCommandContext _context;

    public StudioDomainCommandService(StudioCommandContext context)
    {
        _context = context;
    }
        
    public async Task<StudioCreateResponse> CreateStudioAsync(StudioCreateRequest request, CallContext context = default)
    {
        if (request == null || string.IsNullOrEmpty(request.Name))
        {
            var correlationId = Guid.NewGuid();
            // Log issue
            throw new ArgumentNullException($"Id: {correlationId}");
        }

        try
        {
            var studio = new Studio
            {
                Name = request.Name
            };

            _context.Studios.Attach(studio);
            _context.Entry(studio).State = EntityState.Added;
            var updateCount = await _context.SaveChangesAsync();
            
            if(updateCount > 0)
            {
                var response = new StudioCreateResponse
                {
                    Id = studio.Id,
                    Name = studio.Name
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
}
