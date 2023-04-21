using AspNetCoreRateLimit;
using PoC.DoS.Services;
using PoC.DoS.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddHttpClient();
builder.Services.AddHostedService<QueuedHostedService>();
builder.Services.AddSingleton<IBackgroundTaskQueue>(ctx =>
{
     return new BackgroundTaskQueue(100);
});

// Rate limit
// https://github.com/stefanprodan/AspNetCoreRateLimit/wiki/IpRateLimitMiddleware#setup
builder.Services.AddMemoryCache();
var quotaExceededResponse  = new QuotaExceededResponse
{
    StatusCode = 429,
    ContentType = "text/html",
    Content = "Oh no, you've exceeded your rate limit! Please come back later (Hit F5)!"

};

builder.Services.Configure<IpRateLimitOptions>(options =>
{
    options.EnableEndpointRateLimiting = true;

    options.QuotaExceededResponse = quotaExceededResponse;

    options.GeneralRules = new List<RateLimitRule>
    {
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 2,
            Period = "1s"
        },
        new RateLimitRule
        {
            Endpoint = "*",
            Limit = 100,
            Period = "1h"
        },
        new RateLimitRule
        {
            Endpoint = "post:*",
            Limit = 5,
            Period = "1h"
        }
    };
});



builder.Services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
builder.Services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();


builder.Services.AddTransient<IDoSService, DoSService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");    
}

app.UseIpRateLimiting();

app.UseStaticFiles();
app.UseRouting();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
