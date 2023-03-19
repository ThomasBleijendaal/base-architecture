var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddCommonServices()
    .AddServices();

builder.Services
    .AddSingleton<IObjectModelValidator, ValidatedObjectModelValidator>();

builder.Services
    .AddControllers(config =>
    {
        config.ModelBinderProviders.Insert(0, new ValidatedModelBinderProvider());

        config.Filters.Add<ValidatedContentFilter>();
    });

builder.Services
    .AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.MapControllers();

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

app.Run();
