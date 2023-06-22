using Microsoft.AspNetCore.Mvc;
using MovieOnlineBookingMVC.Models;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;

namespace MovieOnlineBookingMVC.Controllers
{
    public class MovieController : Controller
    {
        string  baseAddress = "https://localhost:7101/api/User/Movie/";      //base url
       
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            DataTable movieList = new DataTable();
                       
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(baseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync("GetMovies");

                if (response.IsSuccessStatusCode)
                {
                    string data = response.Content.ReadAsStringAsync().Result;
                    movieList = JsonConvert.DeserializeObject<DataTable>(data);
                }
                else
                {
                    Console.WriteLine("Error to fetch web api");
                }
                ViewData.Model = movieList;
            }
            return View();
        }
    }
}
