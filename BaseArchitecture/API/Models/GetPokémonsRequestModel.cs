namespace API.Models;

public record GetPokémonsRequestModel(int Level);
public class GetPokémonsRequest
{
    [FromQuery] public int Level { get; set; }
}
