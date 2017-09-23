using SnapShop.Design.Common;
using SnapShop.Design.Common.Response;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web.Http;
using System.Web.Script.Serialization;

namespace SnapShop.Design.Controllers
{
    public class CallsController : ApiController
    {
        // GET: api/Calls
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Calls/5
        public string Get(int id)
        {
            return "value";
        }
         
        // PUT: api/Calls/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Calls/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("api/Calls/UploadFile/")]
        public List<Labelannotation> UploadFile([FromBody]Common.Request.APIParameters base64ImageString)
        {
            try
            {
                List<Labelannotation> returnResult = new List<Labelannotation>();
                Rootobject responseData = null;
                JavaScriptSerializer obj = new JavaScriptSerializer();
                CloudVisionAPIClient APIClient = new CloudVisionAPIClient();
                string json = APIClient.buildJsonFile(base64ImageString.base64ImageString, FeatureType.LABEL_DETECTION, 100);
                using (var client = new HttpClient())
                {
                    string responseResult = string.Empty;
                    client.BaseAddress = new Uri($"https://vision.googleapis.com/v1/images:annotate?key=" + Convert.ToString(ConfigurationManager.AppSettings["APIKey"]));
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.PostAsync("", new StringContent(json, Encoding.UTF8, "application/json")).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        responseResult = response.Content.ReadAsStringAsync().Result;
                        responseData = obj.Deserialize<Rootobject>(responseResult);
                        foreach (var responses in responseData.responses)
                        {
                            returnResult = responses.labelAnnotations.OrderByDescending(x => x.score).ToList();
                        }

                        return returnResult;

                    }
                    else
                    {
                        return returnResult;
                    }
                }
            }
            catch
            {

                throw;
            }

        }
    }
}
