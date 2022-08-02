using Grpc.Core;
using ProtoBuf.Grpc;
using StudioDomain.Command.Service.Data;
using StudioDomain.Common.Data.Entities;
using StudioDomain.Shared.Commands.v1.Contracts;
using StudioDomain.Shared.Commands.v1.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace StudioDomain.Command.Service.Services.v1;

public class StudioDomainCommandService : IStudioDomainCommandService
{
    private readonly ILogger _logger;
    private readonly StudioCommandContext _context;

    public StudioDomainCommandService(ILogger<StudioDomainCommandService> logger, StudioCommandContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<StudioDeleteResponse> DeleteStudioAsync(StudioDeleteRequest request, CallContext context = default)
    {
        if (request == null || request.Id <= 0)
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
                return new StudioDeleteResponse { Success = false };
            }

            _context.Studios.Remove(studio);
            _context.Entry(studio).State = EntityState.Deleted;
            var removeCount = _context.SaveChanges();
            if (removeCount == 0)
            {
                return new StudioDeleteResponse { Success = false };
            }

            return new StudioDeleteResponse { Success = true };
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
        
    public async Task<StudioCreateResponse> CreateStudioAsync(StudioCreateRequest request, CallContext context = default)
    {
        if (request == null || string.IsNullOrEmpty(request.Name))
        {
            var metadata = new Metadata
            {
                { "CorrelationId", Guid.NewGuid().ToString() }
            };

            throw new RpcException(new Status(StatusCode.InvalidArgument, nameof(request)), metadata);
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
}
