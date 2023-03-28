namespace API.Controllers;

public class PokémonController : Controller
{
    private readonly IMediator _mediator;

    public PokémonController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/pokemon/{name}")]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetAsync(
        [FromRoute] Validated<GetPokémonRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonQuery(request.Value.Name));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        if (result.Value == null)
        {
            return NotFound();
        }

        return Ok(Map(result.Value));
    }

    /// <summary>
    /// This method disables the model validation for Validated&lt;T&gt;, allow it to handle the
    /// validation results in the action body, or ignoring it and trigger a ValidationException
    /// when accessing .Value without checking .IsValid first.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpGet("/pokemon/danger/{name}")]
    [AllowInvalidModel]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetDangerouslyAsync(
        [FromRoute] Validated<GetPokémonRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonQuery(request.Value.Name));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        if (result.Value == null)
        {
            return NotFound();
        }

        return Ok(Map(result.Value));
    }

    [HttpPost("/pokemon/search")]
    public async Task<ActionResult<IReadOnlyList<PokémonSearchResponseModel>>> SearchAsync(
        [FromBody] Validated<SearchPokémonRequestModel> request)
    {
        var result = await _mediator.Send(new SearchPokémonQuery(request.Value.Name, request.Value.Height, request.Value.Weight));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        return Ok(MapSearch(result.Value));
    }

    [HttpPost("/pokemon/like")]
    public async Task<ActionResult> GetAsync(
        [FromQuery] Validated<LikePokémonRequestModel> request)
    {
        var result = await _mediator.Send(new LikePokémonCommand(request.Value.Id));

        if (!result.IsSuccess)
        {
            return StatusCode(StatusCodes.Status500InternalServerError);
        }

        return NoContent();
    }

    private static IReadOnlyList<PokémonSearchResponseModel>? MapSearch(IReadOnlyList<Pokémon>? pokémons)
        => pokémons?.Select(x => MapSearch(x)).ToList();

    [return: NotNullIfNotNull(nameof(pokémon))]
    private static PokémonResponseModel? Map(Pokémon? pokémon)
        => pokémon == null ? null : new(pokémon.Id, pokémon.Name, pokémon.Weight, pokémon.Height, pokémon.NrOfLikes);

    [return: NotNullIfNotNull(nameof(pokémon))]
    private static PokémonSearchResponseModel? MapSearch(Pokémon? pokémon)
        => pokémon == null ? null : new(pokémon.Id, pokémon.Name, pokémon.Weight, pokémon.Height);
}
