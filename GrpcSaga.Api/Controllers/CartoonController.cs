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
    private readonly ICartoonApiService _apiService;

    public CartoonalogueController(ILogger<CartoonalogueController> logger, ICartoonApiService apiService)
    {
        _logger = logger;
        _apiService = apiService;
    }

    /// <summary>
    /// Returns a specifc Cartoon
    /// </summary>
    /// <param name="id">The id of the cartoon to return</param>
    /// <returns>The requested CartoonViewModel</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /Cartoon/1
    /// </remarks>
    /// <response code="200">Returns the request was successful</response>
    /// <response code="400">The specified id is invalid</response>
    /// <response code="404">The specified Cartoon Id is not found</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpGet()]
    [Route("Cartoon/{id}")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartoonViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetCartoonById(int id)
    {
        if (id <= 0)
        {
            return BadRequest();
        }

        try
        {
            var viewModelResponse = await _apiService.GetCartoonByIdAsync(id);

            if (viewModelResponse == null)
            {
                return NotFound();
            }
            
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Gets the specified Cartoon, its characters, and studio name
    /// </summary>
    /// <param name="id">The unique Id for the cartoon.</param>
    /// <returns>The the specified CartoonDetailsViewModel (The specified Cartoon, a list of it's Characters, and the Studio name.)</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET /Cartoon/Info/3
    ///     
    /// </remarks>
    /// <response code="200">Returns a CartoonDetailsViewModel object for the specified id</response>
    /// <response code="400">If the id is less than 1</response>
    /// <response code="404">If the specified Cartoon Id is not found</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [Consumes("text/plain")]
    [Produces("application/json")]
    [HttpGet()]
    [Route("Cartoon/{id}/Details")]
    public async Task<IActionResult> GetCartoonDetailsById(int id)
    {
        if (id < 1)
        {
            return BadRequest();
        }

        try
        {
            var viewModelResponse = await _apiService.GetCartoonDetailsByIdAsync(id);

            if (viewModelResponse == null)
            {
                return NotFound();
            }
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Gets a list of all cartoons
    /// </summary>
    /// <returns>List of CartoonViewModel</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     GET Cartoon/List
    /// </remarks>
    /// <response code="200">The request was successful</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpGet]
    [Route("Cartoon/List")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<CartoonViewModel>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> GetCartoonListAsync()
    {
        try
        {
            var viewModelResponse = await _apiService.GetCartoonListAsync();

            if (viewModelResponse == null)
            {
                return NotFound();
            }
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // Log exception info
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }

    /// <summary>
    /// Creates a new Cartoon
    /// </summary>
    /// <param name="viewModel">An instance of a CartoonCreateViewModel</param>
    /// <returns>The newly creates Cartoon</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST Cartoon/Create
    ///     
    /// </remarks>
    /// <response code="200">If the creation was successfule. Returns a CartoonViewModel object for the specified id</response>
    /// <response code="400">If the StudioCreateViewModel is invalid</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpPost]
    [Route("Cartoon/Create")]
    public async Task<IActionResult> CreateCartoonAsync(CartoonCreateViewModel viewModel)
    {
        try
        {
            var viewModelResponse = await _apiService.CreateCartoonAsync(viewModel);
            if (viewModelResponse == null)
            {
                return NoContent();
            }
            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            // log exception
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Data["CorrelationId"].ToString());
        }
    }

    /// <summary>
    /// Creates a specifc Cartoon, it's characters, and studio. This endpoint uses a Saga to compensate across domains
    /// </summary>
    /// <param name="viewModel">An instance of the CartoonDetailsCreateViewModel class</param>
    /// <returns>The newly created CartoonDetails</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST /Cartoon/CreateDetails
    /// </remarks>
    /// <response code="200">Returns the request was successful</response>
    /// <response code="204">The requested cartoon, characters, or studio could not be created</response>
    /// <response code="400">The specified id is invalid</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpPost()]
    [Route("Cartoon/CreateDetails")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartoonDetailsViewModel))]
    [ProducesResponseType(StatusCodes.Status204NoContent, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateCartoonDetails(CartoonDetailsCreateViewModel viewModel)
    {
        var viewModelResponse = await _apiService.CreateCartoonDetailsAsync(viewModel);
        if (viewModelResponse == null)
        {
            return NoContent();
        }
        return Ok(viewModelResponse);
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
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpPut]
    [Route("Cartoon/Update")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CartoonDetailsViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> UpdateCartoon(CartoonUpdateViewModel viewModel)
    {
        if (viewModel == null || !ModelState.IsValid)
        {
            var correlationId = Guid.NewGuid();
            // Log issue
            return BadRequest($"Id: {correlationId}");
        }

        try
        {
            var viewModelResponse = await _apiService.UpdateCartoonAsync(viewModel);

            if (viewModelResponse == null)
            {
                var correlationId = Guid.NewGuid();
                return NotFound($"Id: {correlationId}");
            }

            return Ok(viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // log exception
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Data["CorrelationId"].ToString());
        }
    }

    /// <summary>
    /// Creates a new Studio
    /// </summary>
    /// <param name="viewModel">An instance of a StudioCreateViewModel</param>
    /// <returns>The newly create Studio</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST Studio/Create
    ///     
    /// </remarks>
    /// <response code="200">If the creation was successfule. Returns a StudioViewModel object for the specified id</response>
    /// <response code="400">If the StudioCreateViewModel is invalid</response>
    /// <response code="500">Unkown error. Check error logs</response>
    [HttpPost]
    [Route("Studio/Create")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(StudioViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]    
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateStudioAsync(StudioCreateViewModel viewModel)
    {
        try
        { 
            var viewModelResponse = await _apiService.CreateStudioAsync(viewModel);
            if (viewModelResponse == null)
            {
                return NoContent();
            }

            return StatusCode(StatusCodes.Status201Created, viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // log exception
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Data["CorrelationId"].ToString());
        }
    }

    /// <summary>
    /// Creates a new Character
    /// </summary>
    /// <param name="viewModel">An instance of a CharacterCreateViewModel</param>
    /// <returns>The newly created Character</returns>
    /// <remarks>
    /// Sample request:
    /// 
    ///     POST Character/Create
    ///     
    /// </remarks>
    /// <response code="200">If the creation was successfule. Returns a CharacterViewModel object for the specified id</response>
    /// <response code="400">If the StudioCreateViewModel is invalid</response>
    /// <response code="500">Unkown error. Check error logs</response>[HttpPost]
    [Route("Character/Create")]
    [Consumes("application/json")]
    [Produces("application/json")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(CharacterViewModel))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(string))]
    public async Task<IActionResult> CreateCharacterAsync(CharacterCreateViewModel viewModel)
    {
        try
        {
            var viewModelResponse = await _apiService.CreateCharacterAsync(viewModel);
            if (viewModelResponse == null)
            {
                return NoContent();
            }
            return StatusCode(StatusCodes.Status201Created, viewModelResponse);
        }
        catch (Exception ex)
        {
            if (!ex.Data.Contains("CorrelationId"))
            {
                ex.Data.Add("CorrelationId", Guid.NewGuid().ToString());
            }
            // log exception
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Data["CorrelationId"].ToString());
        }
    }

}