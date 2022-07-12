﻿using Grpc.Core;
using ProtoBuf.Grpc;
using CartoonDomain.Shared.v1.Interfaces;
using CartoonDomain.Shared.Queries.v1.Contracts;
using CartoonDomain.Service.Data;

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
            return response;
                
                //var shows = new List<CartoonSingleResponse>();
            //shows.Add(new CartoonSingleResponse
            //{
            //    Id = 1,
            //    Title = "Rocky and Friends",
            //    YearBegin = 1959,
            //    YearEnd = 1963,
            //    Description = "The adventures of Moose and Squirrel as they fight the bad guys from Potsylvania.",
            //    Rating = 5.0m
            //});

            //shows.Add(new CartoonSingleResponse
            //{
            //    Id = 2,
            //    Title = "The Simpsons",
            //    YearBegin = 1989,
            //    Description = "Just your typical American family.",
            //    Rating = 5.0m
            //});


            //return shows[request.Id - 1];
        }
        catch (Exception ex)
        {
            var correlationId = Guid.NewGuid();
            // Log issue and correlation Id
            throw new RpcException(new Status(StatusCode.Unknown, $"Id: {correlationId}"));
        }
    }
}
