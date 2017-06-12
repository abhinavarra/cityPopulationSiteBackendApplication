using GetAllCity.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Web.Script.Serialization;

namespace GetAllCity.Controllers
{
    // Allow CORS for all origins. (Caution!)
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class ProductController : ApiController
    {
        Product[] products = new Product[]
        {
            new Product {id = 1, productName = "prod1", productDescription = "prod des1" },
            new Product {id = 2, productName = "prod2", productDescription = "prod des2" },
            new Product {id = 3, productName = "prod3", productDescription = "prod des3" },
            new Product {id = 4, productName = "prod4", productDescription = "prod des4" },
        };
        [Route("api/product")]
        public IEnumerable<Product> GetAllProducts()
        {
            return products;
        }

        [Route("api/product/{iD}")]
        public IHttpActionResult GetProduct(int iD)
        {
            var product = products.FirstOrDefault((p)=>p.id == iD);
            if(product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet]
        [Route("api/cities")]
        public HttpResponseMessage GetCities()
        {
            var stream = new FileStream(@"E:\GetAllCity\GetAllCity\CitiesJson\zips.json", FileMode.Open);

            var getCities = Request.CreateResponse(HttpStatusCode.OK);
            getCities.Content = new StreamContent(stream);
            getCities.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            return getCities;
        }
        [Route("api/cities/{paramCity}")]
        public HttpResponseMessage GetCity(string paramCity)
        {
            //var stream = new FileStream(@"E:\GetAllCity\GetAllCity\CitiesJson\zips.json", FileMode.Open);

            var getCities = Request.CreateResponse(HttpStatusCode.OK);
            //getCities.Content = new StreamContent(stream);
            //getCities.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            CityDetails obj = new CityDetails();
            using (StreamReader r = new StreamReader(@"E:\GetAllCity\GetAllCity\CitiesJson\zips.json"))
            {
                string json = r.ReadToEnd();
                json = json.Replace(@"\", "");
                json = json.Replace(@"\n", "");
                var d = new JsonDeserializer(json);
                //List<CityDetails> items = JsonConvert.DeserializeObject<List<CityDetails>>(json);
                //dynamic array = JsonConvert.DeserializeObject(json);
                
                //foreach (var item in array)
                //{
                //    if (item.city == paramCity)
                //    {
                //        obj = item;
                //        break;
                //    }
                //}

            }
            //obj;
            return getCities;

           // return getCities;
        }

    }
   

    public class JsonDeserializer
    {
        private IDictionary<string, object> jsonData { get; set; }

        public JsonDeserializer(string json)
        {
            var json_serializer = new JavaScriptSerializer();

            jsonData = (IDictionary<string, object>)json_serializer.DeserializeObject(json);
        }

        public string GetString(string path)
        {
            return (string)GetObject(path);
        }

        public int? GetInt(string path)
        {
            int? result = null;

            object o = GetObject(path);
            if (o == null)
            {
                return result;
            }

            if (o is string)
            {
                result = Int32.Parse((string)o);
            }
            else
            {
                result = (Int32)o;
            }

            return result;
        }

        public object GetObject(string path)
        {
            object result = null;

            var curr = jsonData;
            var paths = path.Split('.');
            var pathCount = paths.Count();

            try
            {
                for (int i = 0; i < pathCount; i++)
                {
                    var key = paths[i];
                    if (i == (pathCount - 1))
                    {
                        result = curr[key];
                    }
                    else
                    {
                        curr = (IDictionary<string, object>)curr[key];
                    }
                }
            }
            catch
            {
                // Probably means an invalid path (ie object doesn't exist)
            }

            return result;
        }
    }
}
