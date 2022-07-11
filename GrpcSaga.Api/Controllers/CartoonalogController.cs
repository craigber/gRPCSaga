using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ShowDomain.Shared.v1.Contracts;
using ShowDomain.Shared.v1.Interfaces;
using GrpcSaga.Api.ViewModels;

namespace GrpcSaga.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CartoonController : ControllerBase
{
    
    private readonly ILogger<CartoonController> _logger;

    public CartoonController(ILogger<CartoonController> logger)
    {
        _logger = logger;
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
            var request = new ShowGetSingleRequest
            {
                Id = id
            };

            var channel = GrpcChannel.ForAddress("https://localhost:7227");
            var client = channel.CreateGrpcService<IShowService>();

            var getShowResponse = await client.GetShow(request);

            var viewModelResponse = new CartoonViewModel
            {
                Id = getShowResponse.Id,
                Name = getShowResponse.Name,
                YearBegin = getShowResponse.YearBegin,
                YearEnd = getShowResponse.YearEnd,
                StudioId = getShowResponse.StudioId
            };

            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}