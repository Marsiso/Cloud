using Cloud.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

services.AddControllers();
services.AddLocalization(options => options.ResourcesPath = "Resources");

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddViewModels();

services.AddMudServices();

services.AddSqliteDatabaseSession(config, env);

var app = builder.Build();

app.UseSqliteInitializer();

if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseLocalizationResources();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
