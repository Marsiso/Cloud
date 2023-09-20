var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;
var env = builder.Environment;

services.AddRazorPages();
services.AddServerSideBlazor();

var app = builder.Build();

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

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
