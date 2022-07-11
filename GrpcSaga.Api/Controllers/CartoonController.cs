using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ShowDomain.Shared.v1.Contracts;
using ShowDomain.Shared.v1.Interfaces;
using GrpcSaga.Api.ViewModels;
using GrpcSaga.Api.Services;

namespace GrpcSaga.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CartoonController : ControllerBase
{
    
    private readonly ILogger<CartoonController> _logger;
    private readonly ICartoonService _cartoonService;

    public CartoonController(ILogger<CartoonController> logger, ICartoonService cartoonService)
    {
        _logger = logger;
        _cartoonService = cartoonService;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCartoonById(int id)
    {
        if (id <= 0)
        {
            var correlationId = Guid.NewGuid();
            // log issue and correlation Id
            return BadRequest($"Id: {correlationId}");
        }

        try
        {
            var viewModelResponse = await _cartoonService.GetCartoonByIdAsync(id); 

            if (viewModelResponse == null)
            {
                var correlationId = Guid.NewGuid();
                // log Not Found
                return NotFound(($"Id: {correlationId}"));
            }
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}