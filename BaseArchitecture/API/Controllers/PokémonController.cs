using API.Extensions;

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
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Level));

        if (!result.IsSuccess)
        {
            return result.GetDefaultUnsuccessfulResult();
        }

        return Ok(result.Value);
    }

    [HttpPost("/type")]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetListByBodyAsync(
        [FromBody] Validated<GetPokémonsRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Level));

        if (!result.IsSuccess)
        {
            return result.GetDefaultUnsuccessfulResult();
        }

        return Ok(result.Value);
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
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Level));

        if (!result.IsSuccess)
        {
            return result.GetDefaultUnsuccessfulResult();
        }

        return Ok(result.Value);
    }
}
