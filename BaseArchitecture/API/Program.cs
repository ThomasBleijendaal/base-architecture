// TODO: add function app
// TODO: add service bus
// TODO: add repository / db
// TODO: add documentation about what each layer does and where they fit in the architecture
// TODO: add a POST request

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApiServices();

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
