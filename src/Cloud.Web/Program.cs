using Cloud.Web;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var configuration = builder.Configuration;
var environment = builder.Environment;

services.AddControllers();
services.AddControllersWithViews();
services.AddLocalization(options => options.ResourcesPath = "Resources");

services.AddRazorPages();
services.AddServerSideBlazor();
services.AddViewModels();
services.AddUtilities(configuration);

services.AddMudServices();

services.AddSqliteDatabaseSession(configuration, environment);

services.AddIdentityProvider(configuration);

var application = builder.Build();

application.UseSqliteInitializer();

if (environment.IsDevelopment())
{
    application.UseDeveloperExceptionPage();
}
else
{
    application.UseExceptionHandler("/Error");
    application.UseHsts();
}

application.UseHttpsRedirection();
application.UseStaticFiles();

application.UseLocalizationResources();

application.UseRouting();

application.UseAuthentication();

application.MapControllers();
application.MapBlazorHub();
application.MapFallbackToPage("/_Host");

application.Run();
