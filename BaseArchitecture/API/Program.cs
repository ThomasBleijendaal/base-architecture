using Common.Binding;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddSingleton(typeof(JsonModelBinder<>));

builder.Services
    .AddValidatorsFromAssemblyContaining<Program>();

builder.Services
    .AddCommonServices()
    .AddServices();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapGet("/type",
    async ([AsParameters] GetPokémonsRequest request, [FromServices] IMediator mediator)
    =>
    {
        return Results.Ok(request.Level);

        //if (!request.IsValid)
        //{
        //    return Results.BadRequest(request.Errors);
        //}

        //var result = await mediator.Send(new GetPokémonsQuery(request.Value.Level));

        //return Results.Ok(result);
    });

app.MapPost("/type",
    async (Validated<GetPokémonsRequestModel> request, [FromServices] IMediator mediator)
    =>
    {
        if (!request.IsValid)
        {
            return Results.BadRequest(request.Errors);
        }

        var result = await mediator.Send(new GetPokémonsQuery(request.Value.Level));

        return Results.Ok(result);
    });

app.Run();
