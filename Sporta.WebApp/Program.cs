using Microsoft.AspNetCore.Builder;
using Sporta.WebApp;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup(builder.Configuration);

startup.ConfigureServices(builder.Services);

var app = builder.Build();

startup.Configure(app, app.Environment);
app.MapRazorPages();
await app.RunAsync();