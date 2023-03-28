// TODO: add function app

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
