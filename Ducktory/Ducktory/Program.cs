using AutoMapper;
using DuckLibrary;
using DuckLibrary.Services;
using Ducktory.Components;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMudServices();

builder.Services.AddAutoMapper(typeof(InitilizeMappings).Assembly);
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<CacheService>();
builder.Services.AddSingleton<CmsService>();
builder.Services.AddScoped<DuckService>();
builder.Services.AddHttpClient();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddControllers();
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("images-public", p => p
        .AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod());
});
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseRouting();
app.UseCors("images-public");
app.MapControllers();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();