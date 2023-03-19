namespace API.Models;

public class GetPokémonsRequest
{
    [FromQuery] public int Level { get; set; }
}
