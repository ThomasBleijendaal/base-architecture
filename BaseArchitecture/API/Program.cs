

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddValidatorsFromAssemblyContaining<Program>();

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

app.MapGet("/type/{level:int}", ([AsParameters]Validated<GetPokémonTypeCollectionRequestModel> request, [FromServices]IPokeGateway gateway) 
    => gateway.GetPokémonAsync(request.Value.Level));

app.Run();
