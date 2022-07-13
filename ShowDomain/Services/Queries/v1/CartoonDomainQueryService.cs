﻿using Grpc.Core;
using ProtoBuf.Grpc;
using CartoonDomain.Shared.v1.Interfaces;
using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Service.Data;
using CartoonDomain.Service.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CartoonDomain.Service.Services.v1;

public class CartoonDomainQueryService : ICartoonDomainQueryService
{
    private readonly CartoonQueryContext _context;

    public CartoonDomainQueryService(CartoonQueryContext context)
    {
        _context = context;
    }

    public async Task<CartoonSingleResponse> GetCartoonByIdAsync(CartoonSingleRequest request, CallContext context = default)
    {
        if (request.Id <= 0)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.NotFound, $"Id: {correlationId}"));
        }

        try
        {
            var cartoon = await _context.Cartoons.FindAsync(request.Id);

            if (cartoon == null)
            {
                return null;
            }

            var response = MapToSingleResponse(cartoon);
            return response;

        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.Unknown, $"Id: {correlationId}"));
        }
    }

    public async Task<CartoonMultipleResponse> GetAllCartoonsAsync(CallContext context = default)
    {
        try
        {
            var cartoons = _context.Cartoons.Include("Characters").ToList();

            if (cartoons == null || cartoons.Count == 0)
            {
                return null;
            }

            var response = new CartoonMultipleResponse();
            response.Cartoons = new List<CartoonSingleResponse>();
            foreach (var c in cartoons)
            {
                response.Cartoons.Add(MapToSingleResponse(c));
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

    private CartoonSingleResponse MapToSingleResponse(Cartoon cartoon)
    {
        var response = new CartoonSingleResponse
        {
            Id = cartoon.Id,
            Title = cartoon.Title,
            YearBegin = cartoon.YearBegin,
            YearEnd = cartoon.YearEnd,
            Description = cartoon.Description,
            Rating = cartoon.Rating,
            StudioId = cartoon.StudioId        
        };

        if (cartoon.Characters.Any())
        {
            response.Characters = new List<CharacterSingleResponse>();
            foreach (var character in cartoon.Characters)
            {
                response.Characters.Add(new CharacterSingleResponse
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
