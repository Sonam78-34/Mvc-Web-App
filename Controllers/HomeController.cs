using Microsoft.AspNetCore.Mvc;
using MovieOnlineBookingMVC.Models;
using Newtonsoft.Json;
using System.Data;
using System.Diagnostics;
using System.Net.Http.Headers;
using System.Text;

namespace MovieOnlineBookingMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
/*
        public IActionResult Index()
        {
            return View();
        }
*/
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();

        }



        [HttpPost]
        public async Task<IActionResult> Register(Register user)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Register");
            }

            var registeredUsers = await GetRegisteredUsers();

            if (registeredUsers.Any(u => u.Name == user.Name))
            {
                ModelState.AddModelError(string.Empty, "Username already exists.");
                return RedirectToAction("Register");
            }
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/api/Authentication/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.PostAsJsonAsync("?role=User", user);
                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }



                   //for getRegisteredUser function from admin api
        [HttpGet]
        public async Task<List<Register>> GetRegisteredUsers()
        {
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7101/api/Admin/UserRegister/");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = await client.GetAsync("GetUserRegistrationDetails");
                if (response.IsSuccessStatusCode)
                {
                    string contentString = await response.Content.ReadAsStringAsync();
                    var users = JsonConvert.DeserializeObject<List<Register>>(contentString);
                    return users;
                }
                else
                {

                    return new List<Register>();
                }
            }
        }

        //Login
        public IActionResult Login()
        {

            return View();
        }

        //[HttpPost]
        //public async Task<IActionResult> Login(Login login)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        Console.WriteLine("LoginState is not valid");
        //        return RedirectToAction("Login");

        //    }

        //    var registeredUsers = await GetRegisteredUsers();
        //    if (IsValidUser(login.UserName, login.Password, registeredUsers))
        //    {
        //        Console.WriteLine("LoginSuccessful");
        //        return RedirectToAction("Index");         //home Page movie Display
        //    }
        //    else
        //    {

        //        ModelState.AddModelError(string.Empty, "Invalid email or password.");
        //        Console.WriteLine("Login is not avalid");
        //        return RedirectToAction("Login");
        //    }
        //}

        //private bool IsValidUser(string name, string password, List<Register> registeredUsers)
        //{
        //    var user = registeredUsers.FirstOrDefault(u => u.Name == name && u.Password == password);
        //    return user != null;
        //}


        [HttpPost]
        public async Task<IActionResult> Login(Login login)
        {
            if (!ModelState.IsValid)
            {
                Console.WriteLine("LoginState is not valid");
                return RedirectToAction("Login");
            }

            var registeredUsers = await GetRegisteredUsers();
            var user = registeredUsers.FirstOrDefault(u => u.Name == login.UserName && u.Password == login.Password);
            if (user != null)
            {
                if (user.Role == "Admin")
                {
                    // Admin login logic
                    Console.WriteLine("Admin Login Successful");
                    return RedirectToAction("AdminDashboard"); // Redirect to admin dashboard page
                }
                else if (user.Role == "User")
                {
                    // User login logic
                    Console.WriteLine("User Login Successful");
                    return RedirectToAction("UserDashboard"); // Redirect to user dashboard page
                }
            }

            ModelState.AddModelError(string.Empty, "Invalid email or password.");
            Console.WriteLine("Login is not valid");
            return RedirectToAction("Login");
        }




        //For showing the movie list

        [HttpGet]
        public async Task<IActionResult> Index()
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






        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}