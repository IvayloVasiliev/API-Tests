using NUnit.Framework;
using RestSharp;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace POMHomework.WebServices
{
    public class APITests
    {
        private HttpClient _client;

        [SetUp]
        public void SetUp()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri("http://localhost:3000");
            _client.DefaultRequestHeaders.Add("G-Token", "ROM831ESV");
        }

        [Test]
        public void CreateHouseholdsWithRestSharp()
        {
            var client = new RestClient("http://localhost:3000/households");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("G-Token", "ROM831ESV");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\n\t\"name\": \"Pernik\"\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
                                    
            Assert.That(response.Content.Contains("Pernik"));
        }

        [Test]
        public void CreateUserWithRestSharp()
        {
            var client = new RestClient("http://localhost:3000/users");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("G-Token", "ROM831ESV");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "{\r\n\t\"email\":\"nagosho@gmail.com\",\r\n\t\"firstName\":\"Gosho\",\r\n\t\"lastName\":\"Goshev\",\r\n\t\"householdId\":\"1\" \r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            
        }

        [Test]
        public void CreateBookWithRestSharp()
        {
            var client = new RestClient("http://localhost:3000/books");
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);
            request.AddHeader("G-Token", "ROM831ESV");
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("application/json", "  {\r\n    \"title\": \"The Bible\",\r\n    \"author\": \"God\",\r\n     \"isbn\": \"0875520988\",\r\n    \"publicationDate\": \"1995-06-30\"\r\n}", ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
           
        }

        [Test]
        public async Task GetHousehold()
        {
            var expectedHousehold = new Household
            {
                Name = "Sofia"
            };
           
            var responce = await _client.GetAsync("/households/1");
            var resAsString = responce.Content.ReadAsStringAsync().Result;

            var household = Household.FromJson(resAsString);

            Assert.AreEqual(expectedHousehold.Name, household.Name);
        }

        [Test]
        public async Task GetHouseholdNegative()
        {
            var responce = await _client.GetAsync("/households/101");
            
            Assert.AreEqual(HttpStatusCode.NotFound, responce.StatusCode);
        }

        [Test]
        public async Task PostHouseholdClass()
        {
            var postHousehold = new Household
            {
                Name = "New York"
            };

            var expectedHousehold = new Household
            {
                Name = "New York"
            };

            var content = new StringContent(postHousehold.ToJson(), Encoding.UTF8, "application/json");

            var responce = await _client.PostAsync("/households", content);
            responce.EnsureSuccessStatusCode();

            var resAsString = responce.Content.ReadAsStringAsync().Result;
            var actualHousehold = Household.FromJson(resAsString);
            
            Assert.AreEqual(expectedHousehold.Name, actualHousehold.Name);
            Assert.IsNotNull(actualHousehold.Id);
        }

        [Test]
        public async Task PostHouseholdWithoutName()
        {
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");

            var responce = await _client.PostAsync("/households", content);
            Assert.AreEqual(HttpStatusCode.InternalServerError, responce.StatusCode);
        }

        [Test]
        public async Task PostUser()
        {
            var postUser = new User
            {
                    Email = "napesho@gmail.com",
	                FirstName ="Pesho",
	                LastName = "Peshev",
	                HouseholdId = 1
            };

            var expectedUser = new User
            {
                Email = "napesho@gmail.com",
                FirstName = "Pesho",
                LastName = "Peshev",
                HouseholdId = 1
            };

            var content = new StringContent(postUser.ToJson(), Encoding.UTF8, "application/json");

            var responce = await _client.PostAsync("/users", content);
            responce.EnsureSuccessStatusCode();

            var resAsString = responce.Content.ReadAsStringAsync().Result;
            var actualUser = User.FromJson(resAsString);

            Assert.AreEqual(expectedUser.Email, actualUser.Email);
            Assert.AreEqual(expectedUser.FirstName, actualUser.FirstName); 
            Assert.AreEqual(expectedUser.LastName, actualUser.LastName);
            Assert.AreEqual(expectedUser.HouseholdId, actualUser.HouseholdId);
        }

        [Test]
        public async Task PostBook()
        {
            var postBook = new Book
            {
                Title = "A Game of Throns",
                Author = "G RR Martin",
                PublicationDate = "2000-06-03",
                Isbn = "0875558885"
            };

            var expectedBook = new Book
            {
                Title = "A Game of Throns",
                Author = "G RR Martin",
                PublicationDate = "2000-06-03",
                Isbn = "0875558885"
            };

            var content = new StringContent(postBook.ToJson(), Encoding.UTF8, "application/json");

            var responce = await _client.PostAsync("/books", content);
            responce.EnsureSuccessStatusCode();

            var resAsString = responce.Content.ReadAsStringAsync().Result;
            var actualBook = Book.FromJson(resAsString);

            Assert.AreEqual(expectedBook.Title, actualBook.Title);
            Assert.AreEqual(expectedBook.Author, actualBook.Author);
            Assert.AreEqual(expectedBook.PublicationDate, actualBook.PublicationDate);
            Assert.AreEqual(expectedBook.Isbn, actualBook.Isbn);
        }

        //[Test]
        //public async Task GetWishlist()
        //{
        //    var responce = await _client.GetAsync("/wishlists/11/books");
        //    var resAsString = responce.Content.ReadAsStringAsync().Result;

        //    var wishlist = Wishlist.FromJson(resAsString);
        //    var books = wishlist.Books;

        //}

        [Test]
        public void GetWishlistWithRestSharp()
        {
            var client = new RestClient("http://localhost:3000/wishlists/11/books");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            request.AddHeader("G-Token", "ROM831ESV");
            request.AddHeader("Content-Type", "application/json");
            IRestResponse response = client.Execute(request);
            Assert.That(response.Content.Contains("Origin of speceis"));
            Assert.That(response.Content.Contains("Harry Potter")); 

        }

    }
}
