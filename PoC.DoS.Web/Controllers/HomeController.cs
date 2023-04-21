using Microsoft.AspNetCore.Mvc;
using PoC.DoS.Services;
using PoC.DoS.Web.Models;
using PoC.DoS.Web.Services;
using System.Diagnostics;

namespace PoC.DoS.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBackgroundTaskQueue _backgroundTaskQueue;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IDoSService _doSService;

        public HomeController(ILogger<HomeController> logger,
            IBackgroundTaskQueue backgroundTaskQueue,
            IHttpClientFactory httpClientFactory,
            IDoSService doSService
            )
        {
            _logger = logger;
            _backgroundTaskQueue = backgroundTaskQueue;
            _httpClientFactory = httpClientFactory;
            _doSService = doSService;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Index(DoSViewModel request)
        {
            var httpClient = _httpClientFactory.CreateClient();

            if (!ModelState.IsValid)
            {
                return View();
            }

            await _backgroundTaskQueue.QueueBackgroundWorkItemAsync(async (ct) =>
            {
                await _doSService.RunAsync(httpClient, request.Url, request.NumberOfRound);
            });

            return View("Done");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}