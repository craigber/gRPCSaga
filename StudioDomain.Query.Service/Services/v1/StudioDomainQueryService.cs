using ProtoBuf.Grpc;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using StudioDomain.Service.Data;
using StudioDomain.Common.Data.Entities;

namespace StudioDomain.Service.Services.v1;

public class StudioDomainQueryService : IStudioDomainQueryService
{
    private readonly StudioQueryContext _context;

    public StudioDomainQueryService(StudioQueryContext context)
    {
        _context = context;
    }

    public async Task<StudioMultipleResponse> GetAllStudiosAsync(CallContext context = default)
    {
        try
        {
            var studios = _context.Studios.ToList();

            if (studios == null || studios.Count == 0)
            {
                return null;
            }

            var response = new StudioMultipleResponse();
            response.Studios = new List<StudioListResponse>();
            foreach (var s in studios)
            {
                response.Studios.Add(MapToSingleResponse(s));
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

    public async Task<StudioListResponse> GetStudioByIdAsync(StudioRequest request, CallContext context = default)
    {
        if (request.Id <= 0)
        {
            var ex = new ArgumentNullException(nameof(request));
            ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            throw ex;
        }

        try
        {
            var studio = await _context.Studios.FindAsync(request.Id);

            if (studio == null)
            {
                return null;
            }

            var response = MapToSingleResponse(studio);
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

    private StudioListResponse MapToSingleResponse(Studio studio)
    {
        var response = new StudioListResponse
        {
            Id = studio.Id,
            Name = studio.Name
        };

        return response;
    }
}
