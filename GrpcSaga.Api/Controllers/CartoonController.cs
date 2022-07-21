using Microsoft.AspNetCore.Mvc;
using Cartoonalogue.Api.ViewModels;
using Cartoonalogue.Api.Services;

namespace Cartoonalogue.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class CartoonController : ControllerBase
{
    
    private readonly ILogger<CartoonController> _logger;
    private readonly ICartoonApiService _cartoonService;

    public CartoonController(ILogger<CartoonController> logger, ICartoonApiService cartoonService)
    {
        _logger = logger;
        _cartoonService = cartoonService;
    }


    /// <summary>
    /// Updates a specifc Cartoon
    /// </summary>
    /// <param name="viewModel"></param>
    /// <returns>The newly updated Cartoon</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     PUT /UpdateCartoon
    ///     {
    ///         "id": 1,
    ///         "title": "This is my favorite cartoon",
    ///         "yearBegin": 1990,
    ///         "yearEnd": 1998,
    ///         "description": "Lot's of laughs!",
    ///         "rating": 4,
    ///         "studioId": 1
    ///     }
    /// </remarks>
    /// <response code="200">Returns the newly updated item</response>
    /// <response code="400">If the viewModel is null</response>
    /// <response code="404">If the specific Cartoon is not found</response>
    [HttpPut]
    [Route("Update")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartoonViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public async Task<IActionResult> UpdateCartoon(CartoonUpdateViewModel viewModel)
    {
        if (viewModel == null || !ModelState.IsValid)
        {
            var correlationId = Guid.NewGuid();
            // Log issue
            return BadRequest($"Id: {correlationId}");
        }
        var viewModelResponse = await _cartoonService.UpdateCartoonAsync(viewModel);

        if (viewModelResponse == null)
        {
            var correlationId = Guid.NewGuid();
            return NotFound($"Id: {correlationId}");
        }

        return Ok(viewModelResponse);
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

    [HttpGet]
    [Route("GetAll")]
    public async Task<IActionResult> GetAllCartoons()
    {
        try
        {
            var viewModelResponse = await _cartoonService.GetAllCartoonsAsync();

            if (viewModelResponse == null)
            {
                return NotFound();
            }
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpPost]
    [Route("CreateStudio")]
    public async Task<IActionResult> CreateStudioAsync(StudioCreateViewModel viewModel)
    {
        var viewModelResponse = await _cartoonService.CreateStudioAsync(viewModel);
        if (viewModelResponse == null)
        {
            return NoContent();
        }

        return StatusCode(StatusCodes.Status201Created, viewModelResponse);
    }

    [HttpPost]
    [Route("CreateCharacter")]
    public async Task<IActionResult> CreateCharacterAsync(CharacterCreateViewModel viewModel)
    {
        var viewModelResponse = await _cartoonService.CreateCharacterAsync(viewModel);
        if (viewModelResponse == null)
        {
            return NoContent();
        }
        return StatusCode(StatusCodes.Status201Created, viewModelResponse);
    }    
}