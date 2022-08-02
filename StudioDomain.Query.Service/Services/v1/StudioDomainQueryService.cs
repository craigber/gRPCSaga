using Grpc.Core;
using ProtoBuf.Grpc;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using StudioDomain.Service.Data;
using StudioDomain.Common.Data.Entities;

namespace StudioDomain.Service.Services.v1;

public class StudioDomainQueryService : IStudioDomainQueryService
{
    private readonly StudioQueryContext _context;
    private readonly ILogger _logger;

    public StudioDomainQueryService(ILogger<StudioDomainQueryService> logger, StudioQueryContext context)
    {
        _logger = logger;
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
            string correlationId;
            if (ex.Data.Contains("CorrelationId"))
            {
                correlationId = ex.Data["CorrelationId"].ToString();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                ex.Data.Add("CorrelationId", correlationId);
            }
            _logger.LogError(ex.Message, ex);

            var metadata = new Metadata
            {
                { "CorrelationId", correlationId }
            };

            throw new RpcException(new Status(StatusCode.Internal, ex.Message), metadata);
        }
    }

    public async Task<StudioListResponse> GetStudioByIdAsync(StudioRequest request, CallContext context = default)
    {
        if (request.Id <= 0)
        {
            var metadata = new Metadata
            {
                { "CorrelationId", Guid.NewGuid().ToString() }
            };

            throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request)), metadata);
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
            string correlationId;
            if (ex.Data.Contains("CorrelationId"))
            {
                correlationId = ex.Data["CorrelationId"].ToString();
            }
            else
            {
                correlationId = Guid.NewGuid().ToString();
                ex.Data.Add("CorrelationId", correlationId);
            }
            _logger.LogError(ex.Message, ex);

            var metadata = new Metadata
            {
                { "CorrelationId", correlationId }
            };

            throw new RpcException(new Status(StatusCode.Internal, ex.Message), metadata);
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
