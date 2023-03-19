namespace API.Controllers;

public class PokémonController : Controller
{
    private readonly IMediator _mediator;

    public PokémonController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("/type")]
    public async Task<ActionResult<IReadOnlyList<PokémonResponseModel>>> GetListAsync(
        [FromBody] Validated<GetPokémonsRequestModel> request)
    {
        var result = await _mediator.Send(new GetPokémonsQuery(request.Value.Level));

        return Ok(result);
    }

    /// <summary>
    /// This method disables the model validation for Validated<>, allow it to handle the 
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

        return Ok(result);
    }
}
