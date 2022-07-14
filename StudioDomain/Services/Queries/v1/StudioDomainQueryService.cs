using Grpc.Core;
using ProtoBuf.Grpc;
using StudioDomain.Shared.Queries.v1.Interfaces;
using StudioDomain.Shared.Queries.v1.Contracts;
using StudioDomain.Service.Data;
using StudioDomain.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

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
            response.Studios = new List<StudioSingleResponse>();
            foreach (var s in studios)
            {
                response.Studios.Add(MapToSingleResponse(s));
            }
            return response;
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.Unknown, $"Id: {correlationId}"));
        }
    }

    public async Task<StudioSingleResponse> GetStudioByIdAsync(StudioSingleRequest request, CallContext context = default)
    {
        if (request.Id <= 0)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.NotFound, $"Id: {correlationId}"));
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
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.Unknown, $"Id: {correlationId}"));
        }
    }

    private StudioSingleResponse MapToSingleResponse(Studio studio)
    {
        var response = new StudioSingleResponse
        {
            Id = studio.Id,
            Name = studio.Name
        };

        return response;
    }
}
