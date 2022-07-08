using Microsoft.AspNetCore.Mvc;
using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;
using ShowDomain.Shared.v1.Contracts;
using ShowDomain.Shared.v1.Interfaces;
using GrpcSaga.Api.ViewModels;

namespace GrpcSaga.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CartoonalogController : ControllerBase
{
    
    private readonly ILogger<CartoonalogController> _logger;

    public CartoonalogController(ILogger<CartoonalogController> logger)
    {
        _logger = logger;
    }

    // CartoonViewModel
    [HttpGet(Name = "GetCartoon")]
    public async Task<IActionResult> GetCartoon(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var request = new ShowGetSingleRequest
            {
                Id = id
            };

            var channel = GrpcChannel.ForAddress("https://localhost:7227");
            var client = channel.CreateGrpcService<IShowService>();

            var response = await client.GetShow(request);

            var viewModel = new CartoonViewModel
            {
                Id = response.Id,
                Name = response.Name,
                YearBegin = response.YearBegin,
                YearEnd = response.YearEnd,
                StudioId = response.StudioId
            };

            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, ex.ToString());
        }
    }
}