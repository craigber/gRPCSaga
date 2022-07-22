using Microsoft.AspNetCore.Mvc;
using Cartoonalogue.Api.ViewModels;
using Cartoonalogue.Api.Services;

namespace Cartoonalogue.Api.Controllers;

[ApiController]
[Route("api/v1")]
//[Route("[controller]")]
public class CartoonalogueController : ControllerBase
{
    
    private readonly ILogger<CartoonalogueController> _logger;
    private readonly ICartoonApiService _cartoonService;

    public CartoonalogueController(ILogger<CartoonalogueController> logger, ICartoonApiService cartoonService)
    {
        _logger = logger;
        _cartoonService = cartoonService;
    }

    [HttpGet()]
    [Route("Cartoon/{id}")]
    public async Task<IActionResult> GetCartoonById(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var viewModelResponse = await _cartoonService.GetCartoonByIdAsync(id);

            if (viewModelResponse == null)
            {
                return NotFound();
            }
            var viewModel = new CartoonViewModel
            {
                Id = viewModelResponse.Id,
                Title = viewModelResponse.Title,
                Description = viewModelResponse.Description,
                YearBegin = viewModelResponse.YearBegin,
                YearEnd = viewModelResponse.YearEnd,
                Rating = viewModelResponse.Rating,
                StudioId = viewModelResponse.StudioId
            };
            return Ok(viewModel);
        }
        catch (Exception ex)
        {
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    [HttpGet()]
    [Route("Cartoon/Info/{id}")]
    public async Task<IActionResult> GetCartoonInfoById(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var viewModelResponse = await _cartoonService.GetCartoonInfoByIdAsync(id); 

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

    [HttpGet]
    [Route("Cartoon/Info/List")]
    public async Task<IActionResult> GetAllCartoonInfosAsync()
    {
        try
        {
            var viewModelResponse = await _cartoonService.GetAllCartoonInfosAsync();

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
    /// <response code="404">If the specified Cartoon Id is not found</response>
    [HttpPut]
    [Route("Cartoon/Update")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartoonInfoViewModel))]
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

    [HttpPost]
    [Route("Studio/Create")]
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
    [Route("Character/Create")]
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