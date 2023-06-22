using Microsoft.AspNetCore.Mvc;
using MovieOnlineBookingMVC.Models;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System;

using System.Text.Json;
using Newtonsoft.Json;


namespace MovieOnlineBookingMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly HttpClient _httpClient;

        public AdminController()
        {
            _httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }



        [HttpGet]
        public async Task<IActionResult> GetMovie()
        {
            string baseAddress = "https://localhost:7101/api/User/Movie/";
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


        public IActionResult AddMovie()
        {
            return View();
        }


        //for create new movie

        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            try
            {
                string apiUrl = "https://localhost:7101/api/Admin/Movies/AddMovie";

                string movieJson = JsonConvert.SerializeObject(movie);
                HttpContent content = new StringContent(movieJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await _httpClient.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    // Movie posted successfully
                    return RedirectToAction("GetMovie", "Admin"); // Replace "Index" with the appropriate action or view
                }
                else
                {
                    Console.WriteLine("not able to post to movie");
                    // Handle the error case
                    string errorMessage = $"Failed to post movie. StatusCode: {response.StatusCode}";
                    return RedirectToAction("Error", new { message = errorMessage }); // Replace "Error" with the appropriate action or view
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                string errorMessage = $"An error occurred while posting the movie: {ex.Message}";
                return RedirectToAction("Error", new { message = errorMessage }); // Replace "Error" with the appropriate action or view
            }
        }


        //to delete the movie 

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                string apiUrl = $"https://localhost:7101/api/Admin/Movies/DeleteMovieById/{id}";

                HttpResponseMessage response = await _httpClient.DeleteAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    // Movie deleted successfully
                    return RedirectToAction("GetMovie", "Admin");
                }
                else
                {
                    // Handle the error case
                    string errorMessage = $"Failed to delete movie. StatusCode: {response.StatusCode}";
                    return RedirectToAction("Error", new { message = errorMessage });
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                string errorMessage = $"An error occurred while deleting the movie: {ex.Message}";
                return RedirectToAction("Error", new { message = errorMessage });
            }
        }




        //To Edit Movie
        public IActionResult EditMovie()
        {
            return View();

        }

        //function for edit movie 
    
        
       [HttpGet]
       public async Task<IActionResult> EditMovie(string Name, int Id)
       {
           try
           {
               Movie movie= new Movie();   
               string apiUrl = $"https://localhost:7101/api/Admin/Movies/GetMovieByName/{Name}";

               HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

               if (response.IsSuccessStatusCode)
               {
                   string data = response.Content.ReadAsStringAsync().Result;
                    movie = JsonConvert.DeserializeObject<Movie>(data);

                   return View(movie);
               }
               else
               {
                   string errorMessage = $"Failed to fetch movie for update. StatusCode: {response.StatusCode}";
                   Console.WriteLine(errorMessage); // Debug output
                   return RedirectToAction("Error", new { message = errorMessage });
               }
           }
           catch (Exception ex)
           {
               string errorMessage = $"An error occurred while fetching the movie for update: {ex.Message}";
               Console.WriteLine(errorMessage); // Debug output
               return RedirectToAction("Error", new { message = errorMessage });
           }
       }

       

        [HttpPost]
          public async Task<IActionResult> EditMovie(Movie movie)
          {
              try
              {
                  string apiUrl = $"https://localhost:7101/api/Admin/Movies/UpdateMovieById/{movie.MovieId}";

                  string movieJson = JsonConvert.SerializeObject(movie);
                  HttpContent content = new StringContent(movieJson, Encoding.UTF8, "application/json");

                  HttpResponseMessage response = await _httpClient.PutAsync(apiUrl, content);

                  if (response.IsSuccessStatusCode)
                  {
                      
                      return RedirectToAction("GetMovie", "Admin");
                  }
                  else
                  {
                    // Handle the error case
                    Console.WriteLine("Not able to send");
                      string errorMessage = $"Failed to update movie. StatusCode: {response.StatusCode}";
                      return RedirectToAction("Error", new { message = errorMessage });
                  }
              }
              catch (Exception ex)
              {
                  // Handle any exceptions
                  string errorMessage = $"An error occurred while updating the movie: {ex.Message}";
                  return RedirectToAction("Error", new { message = errorMessage });
              }
          }

          
       

    }
}
    
   