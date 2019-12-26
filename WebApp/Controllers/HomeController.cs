using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.Xml;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult WebApiCall()
        {
            IEnumerable<WeatherForecast> returnModel = new List<WeatherForecast>();
            var client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44340/");

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = client.GetAsync("weatherforecast").Result;

            if (response.IsSuccessStatusCode)
            {
                var result = response.Content.ReadAsStringAsync().Result;
                returnModel = JsonConvert.DeserializeObject<IEnumerable<WeatherForecast>>(result);
            }
            else
            {
                Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
            }
            client.Dispose();

            return View(returnModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public string GetWhileLoopData(IEnumerable<WeatherForecast> returnModel)
        {
            string htmlStr = "";

            foreach (var item in returnModel)
            {
                htmlStr += "<tr><td>" + item.Date + "</td><td>" + 
                    item.TemperatureC + "</td><td>" + item.TemperatureF + "</td><td>" + 
                    item.Summary + "</td></tr>";
            }

            return htmlStr;
        }
    }
}
