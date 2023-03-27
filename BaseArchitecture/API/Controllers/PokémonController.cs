using System.Diagnostics.CodeAnalysis;
using Gateways.Poke.Models;

namespace API.Controllers;

public class PokémonController : Controller
{
    private readonly IMediator _mediator;

    public PokémonController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("/type")]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetListByQueryAsync(
        [FromQuery] Validated<GetPokémonsRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Type));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        return Ok(Map(result.Value));
    }

    [HttpPost("/type")]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetListByBodyAsync(
        [FromBody] Validated<GetPokémonsRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Type));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
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
    [HttpPost("/type/danger")]
    [AllowInvalidModel]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetListDangerouslyAsync(
        [FromBody] Validated<GetPokémonsRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Type));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        return Ok(Map(result.Value));
    }

    [HttpGet("/pokemon/{name}")]
    public async Task<ActionResult<IReadOnlyList<PokémonDetailsResponseModel>>> GetAsync(
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
    public async Task<ActionResult<IReadOnlyList<PokémonDetailsResponseModel>>> SearchAsync(
        [FromBody] Validated<SearchPokémonRequestModel> request)
    {
        var result = await _mediator.Send(new SearchPokémonQuery(request.Value.Name, request.Value.Height, request.Value.Weight));

        if (!result.IsSuccess)
        {
            return result.GetDefaultResult();
        }

        return Ok(Map(result.Value));
    }

    private IReadOnlyList<PokémonResponseModel>? Map(IReadOnlyList<Pokémon>? pokémons)
        => pokémons?.Select(x => Map(x)).ToList();

    private IReadOnlyList<PokémonDetailsResponseModel>? Map(IReadOnlyList<PokémonDetails>? pokémons)
        => pokémons?.Select(x => Map(x)).ToList();

    [return: NotNullIfNotNull("pokémon")]
    private PokémonResponseModel? Map(Pokémon? pokémon)
        => pokémon == null ? null : new(pokémon.Id, pokémon.Name);

    [return: NotNullIfNotNull("pokémon")]
    private PokémonDetailsResponseModel? Map(PokémonDetails? pokémon)
        => pokémon == null ? null : new(pokémon.Id, pokémon.Name, pokémon.Weight, pokémon.Height);
}
