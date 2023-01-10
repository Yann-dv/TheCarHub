using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using theCarHub.Models;

namespace theCarHub.Controllers;

public class ApiController : Controller
{
    // API CALL //
    public async Task<ActionResult> Index()
    {
        string BaseUrl = "https://thecarhubapi.azurewebsites.net/";
        List<CarImagesNewModel> ListOfImagesUrl = new List<CarImagesNewModel>();
        using (var client = new HttpClient())
        {
            //Passing service base url
            client.BaseAddress = new Uri(BaseUrl);
            client.DefaultRequestHeaders.Clear();
            //Define request data format
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //Sending request to find web api REST service resource GetAllEmployees using HttpClient
            HttpResponseMessage Res = await client.GetAsync("api/storage/get");
            //Checking the response is successful or not which is sent using HttpClient
            if (Res.IsSuccessStatusCode)
            {
                //Storing the response details recieved from web api
                var EmpResponse = Res.Content.ReadAsStringAsync().Result;
                //Deserializing the response recieved from web api and storing into the Employee list
                ListOfImagesUrl = JsonConvert.DeserializeObject<List<CarImagesNewModel>>(EmpResponse);
            }
            return View(ListOfImagesUrl);
        }
    }
}