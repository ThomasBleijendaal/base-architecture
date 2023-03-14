var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCommonServices()
    .AddPokeGateway();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapGet("/type/{level:int}", ([FromRoute] int level, [FromServices]IPokeGateway gateway) 
    => gateway.GetPokémonAsync(level));

app.Run();
