using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Net.Http.Headers;
using SnapShop.Design.Common;
using System.Text;
using SnapShop.Design.Common.Request;
using SnapShop.Design.MongoDb;
using System.Configuration;
using System.Web.Routing;

namespace SnapShop.Design.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.AppURL = Convert.ToString(ConfigurationManager.AppSettings["AppUrl"]);
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult TopDeals(string data)
        {
            JavaScriptSerializer obj = new JavaScriptSerializer();
            MongoDb<Product> mongoGetRepository = new MongoDb<Product>();
            var result = mongoGetRepository.GetAllProducts();
            return View(result);
        }


        public ActionResult TopDeals1(string data)
        {
            JavaScriptSerializer obj = new JavaScriptSerializer();

            if (!string.IsNullOrEmpty(data))
            {
                var result = obj.Deserialize<List<Product>>(data);
                return RedirectToAction("TopDeals", new RouteValueDictionary(new { controller = "home", action = "TopDeals", data = data}));
                //return View(result);
            }
            else
            {
                MongoDb<Product> mongoGetRepository = new MongoDb<Product>();
                var result = mongoGetRepository.GetAllProducts();
                return View(result);
            }

        }

        public ActionResult UploadSnap()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public string UploadAndCreate(HttpPostedFileBase file)
        {
            foreach (string file1 in Request.Files)
            {
                var postedFile = Request.Files[file1];
                //string fileUploadedPath = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName);
                //postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));

                JavaScriptSerializer obj = new JavaScriptSerializer();
                CloudVisionAPIClient APIClient = new CloudVisionAPIClient();
                //var x = await APIClient.AnnotateImage(fileStream, FeatureType.LABEL_DETECTION, 100);


                var byteArray = new byte[postedFile.InputStream.Length];
                postedFile.InputStream.Read(byteArray, 0, byteArray.Length);
                postedFile.InputStream.Position = 0;
                string base64String = Convert.ToBase64String(byteArray);
                using (var client = new HttpClient())
                {
                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["AppUrl"];
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    APIParameters obj1 = new APIParameters();
                    obj1.base64ImageString = base64String;
                    HttpResponseMessage response = client.PostAsJsonAsync("UploadFile/", obj1).Result;

                    MongoDb<Product> mongoRepository = new MongoDb<Product>();

                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    List<Common.Response.Labelannotation> keywordResults = obj.Deserialize<List<Common.Response.Labelannotation>>(responseResult);
                    List<string> keywords = keywordResults.Select(k => k.description).ToList();
                    CreateProduct(keywords);
                }
            }
            return string.Empty;
        }

        [HttpPost]
        public string UploadAndSearch(HttpPostedFileBase file)
        {
            foreach (string file1 in Request.Files)
            {
                var postedFile = Request.Files[file1];
                //string fileUploadedPath = Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName);
                //postedFile.SaveAs(Server.MapPath("~/UploadedFiles/") + Path.GetFileName(postedFile.FileName));

                JavaScriptSerializer obj = new JavaScriptSerializer();
                CloudVisionAPIClient APIClient = new CloudVisionAPIClient();
                //var x = await APIClient.AnnotateImage(fileStream, FeatureType.LABEL_DETECTION, 100); 

                var byteArray = new byte[postedFile.InputStream.Length];
                postedFile.InputStream.Read(byteArray, 0, byteArray.Length);
                postedFile.InputStream.Position = 0;
                string base64String = Convert.ToBase64String(byteArray);
                using (var client = new HttpClient())
                {
                    string apiUrl = System.Configuration.ConfigurationManager.AppSettings["AppUrl"];
                    client.BaseAddress = new Uri(apiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    APIParameters obj1 = new APIParameters();
                    obj1.base64ImageString = base64String;
                    HttpResponseMessage response = client.PostAsJsonAsync("UploadFile/", obj1).Result;

                    MongoDb<Product> mongoRepository = new MongoDb<Product>();

                    var responseResult = response.Content.ReadAsStringAsync().Result;
                    List<Common.Response.Labelannotation> keywordResults = obj.Deserialize<List<Common.Response.Labelannotation>>(responseResult);
                    List<string> keywords = keywordResults.Select(k => k.description).ToList();
                    var tempResult = GetProductsByKeywords(keywords);
                    //tempResult.All(t => t.Keywords.Contains(keywords))
                    var temp = obj.Serialize(tempResult);
                    return temp;

                }
            }
            return string.Empty;
        }

        private void CreateProduct(List<string> keywords)
        {
            MongoDb<Product> mongoRepository = new MongoDb<Product>();
            Product tempProduct = new Product()
            {
                Keywords = keywords,
                ImageData = ""
            };
            mongoRepository.Add(tempProduct, "test");
        }

        private List<Product> GetProductsByKeywords(List<string> keywords)
        {
            JavaScriptSerializer obj = new JavaScriptSerializer();
            MongoDb<UnwoundProduct> mongoGetRepository = new MongoDb<UnwoundProduct>();
            var result = mongoGetRepository.GetByKeywords(keywords);
            List<Product> products = new List<Product>();
            foreach (UnwoundProduct product in result)
            {
                Product p = new Product();
                p.Name = product.Name;
                p.Description = product.Description;
                p.ImageData = product.ImageData;
                p.Price = product.Price;
                p.Id = product.Id;
                p.Category = product.Category;
                products.Add(p);
            }
            return products;
        }
    }
}