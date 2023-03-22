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

app.Run();
