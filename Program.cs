using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using Town_of_Fairfax.Data;
using Town_of_Fairfax.Data.Context;
using Town_of_Fairfax.Security;

var builder = WebApplication.CreateBuilder(args);

IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(AppDomain.CurrentDomain.BaseDirectory).AddJsonFile("appsettings.json").Build();

builder.Services.AddHttpClient("FairfaxHttpClient").SetHandlerLifetime(Timeout.InfiniteTimeSpan);

bool prodMode = true;

builder.Services.AddSingleton<IConfigurationRoot>(configuration);

builder.Services.AddSingleton<CurrentUser>();


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

builder.Services.AddSession(options =>
{
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromDays(365);
});

builder.Services.AddCors(options =>
{

    options.AddPolicy("FairfaxPolicy", builder =>
    {
        builder.WithOrigins("https://localhost:7095", "https://fairfaxok.com").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });

});


builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});


if(prodMode is true)
{
    builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), options => options.EnableRetryOnFailure(maxRetryCount: 5, maxRetryDelay: System.TimeSpan.FromSeconds(10), errorNumbersToAdd: null)));
}
else
{
    builder.Services.AddDbContext<ApplicationContext>(opt => opt.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=Town-of-fairfax;MultipleActiveResultSets=true"));
}

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddHubOptions(hub => hub.MaximumReceiveMessageSize = 100 * 1024 * 1024).AddCircuitOptions(options => {
    options.DetailedErrors = true;
    options.DisconnectedCircuitRetentionPeriod = TimeSpan.FromSeconds(0);
    options.DisconnectedCircuitMaxRetained = 0;
}); // 100 MB


builder.Services.AddSingleton<IConfiguration>(configuration);

builder.Services.AddControllers();
builder.Services.AddAuthorizationCore();
builder.Services.AddHttpClient();
builder.Services.AddHttpContextAccessor();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



app.UseRouting();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseCookiePolicy();
app.UseAuthentication();
app.UseAuthorization();
app.UseSession();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

app.Run();
