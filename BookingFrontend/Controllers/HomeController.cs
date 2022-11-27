using BookingFrontend.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;

namespace BookingFrontend.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            RestClient restClient = new RestClient("http://localhost:51136/");
            RestRequest rest = new RestRequest("api/Centres", Method.Get);
            RestResponse restResponse = restClient.Execute(rest);

            List<Centre> centres = JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content);   

            return View(centres);
        }

        public IActionResult Admin()
        {
            ViewBag.Title = "Admin";
            return View();
        }

        [HttpGet]
        public IActionResult Search(string name)
        {
            RestClient restClient = new RestClient("http://localhost:51136/");
            RestRequest restRequest = new RestRequest("api/Centres/", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            //Account account1 = JsonConvert.DeserializeObject<Account>(restResponse.Content);
            //account1.Image = this.GetImage(Convert.ToBase64String(account1.Image));
            //return Ok(restResponse.Content);
            List<Centre> centres = JsonConvert.DeserializeObject<List<Centre>>(restResponse.Content);
            List<Centre> newCentre = new List<Centre>();
            foreach(Centre centre in centres)
            {
                if (centre.CnetreName.Contains(name))
                {
                    newCentre.Add(centre);
                }
            }

            return Ok(newCentre);
        }

        [HttpGet]
        public IActionResult Select(int id)
        {
            RestClient restClient = new RestClient("http://localhost:51136/");
            RestRequest restRequest = new RestRequest("api/Bookings", Method.Get);
            RestResponse restResponse = restClient.Execute(restRequest);
            //Account account1 = JsonConvert.DeserializeObject<Account>(restResponse.Content);
            //account1.Image = this.GetImage(Convert.ToBase64String(account1.Image));
            //return Ok(restResponse.Content);
            List<Booking> bookings = JsonConvert.DeserializeObject<List<Booking>>(restResponse.Content);
            List<Booking> newBookings = new List<Booking>();
            foreach (Booking book in bookings)
            {
                if (book.CentreId == id)
                {
                    newBookings.Add(book);
                }
            }

            return Ok(newBookings);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Centre centre)
        {
            RestClient restClient = new RestClient("http://localhost:51136/");
            RestRequest restRequest = new RestRequest("api/Centres", Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(centre));
            RestResponse restResponse = restClient.Execute(restRequest);

            Centre account1 = JsonConvert.DeserializeObject<Centre>(restResponse.Content);
            //account1.Image = this.GetImage(Convert.ToBase64String(account.Image));
            if (account1 != null)
            {
                return Ok(account1);
            }
            else
            {
                return BadRequest();
            }
        }

        [HttpPost]
        public IActionResult addBooking([FromBody] Booking book)
        {
            int id = book.CentreId;
            //List<Booking> bookings1 = new List<Booking>();
            ////bookings = (List<Booking>)Select(id);

            RestClient restClient1 = new RestClient("http://localhost:51136/");
            RestRequest restRequest1 = new RestRequest("api/Bookings", Method.Get);
            RestResponse restResponse1 = restClient1.Execute(restRequest1);
            //Account account1 = JsonConvert.DeserializeObject<Account>(restResponse.Content);
            //account1.Image = this.GetImage(Convert.ToBase64String(account1.Image));
            //return Ok(restResponse.Content);
            List<Booking> bookings = JsonConvert.DeserializeObject<List<Booking>>(restResponse1.Content);
            List<Booking> newBookings = new List<Booking>();
            foreach (Booking bookItem in bookings)
            {
                if (bookItem.CentreId == id)
                {
                    newBookings.Add(bookItem);
                }
            }

            


            DateTime sDate1 = Convert.ToDateTime(book.StartDate);
            DateTime eDate1 = Convert.ToDateTime(book.EndDate);

            foreach (Booking item in newBookings)
            {
                DateTime sDate2 = Convert.ToDateTime(item.StartDate);
                DateTime eDate2 = Convert.ToDateTime(item.EndDate);
                if (sDate1 <= eDate2 && sDate2 <= eDate1)
                {
                    return BadRequest(new Error("Overlapping","Date is already booked\n Please use"+eDate2.AddDays(1)));
                }
            }


            RestClient restClient = new RestClient("http://localhost:51136/");
            RestRequest restRequest = new RestRequest("api/Bookings", Method.Post);
            restRequest.AddJsonBody(JsonConvert.SerializeObject(book));
            RestResponse restResponse = restClient.Execute(restRequest);

            Booking account1 = JsonConvert.DeserializeObject<Booking>(restResponse.Content);
            //account1.Image = this.GetImage(Convert.ToBase64String(account.Image));
            if (account1 != null)
            {
                return Ok(account1);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
