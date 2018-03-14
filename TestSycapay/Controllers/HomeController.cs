using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace TestSycapay.Controllers
{
    public class InitSycapayRequest
    {
        [JsonProperty("X-SYCA-MERCHANDID")]
        public string MerchandId { get; set; }

        [JsonProperty("X-SYCA-APIKEY")]
        public string ApiKey { get; set; }

        [JsonProperty("X-SYCA-REQUEST-DATA-FORMAT")]
        public string RequestDataFormat { get; set; }

        [JsonProperty("X-SYCA-RESPONSE-DATA-FORMAT")]
        public string ResponseDataFormat { get; set; }

        [JsonProperty("Montant")]
        public int Amount { get; set; }

        [JsonProperty("Curr")]
        public string Currency { get; set; }

    }

    public class GetTokenRequest
    {
        [JsonProperty("amount")]
        public int Amount { get; set; }
    }

    public class SycapayResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("desc")]
        public string Description { get; set; }
    }

    [RoutePrefix("home")]
    public class HomeController : Controller
    {

        [Route("gettoken")]
        public JsonResult GetToken(GetTokenRequest getTokenRequest)
        {
            var request = new InitSycapayRequest
            {
                Amount = getTokenRequest.Amount,
                ApiKey = "pk_syca_45f3d8b5ebac2e40fd55c4d198a8d32fed30c494",
                MerchandId = "C_5A95112715F3E",
                Currency = "XOF",
                RequestDataFormat = "JSON",
                ResponseDataFormat = "JSON"
            };


            using (WebClient client = new WebClient())
            {
                client.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
                client.Headers[HttpRequestHeader.Accept] = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                client.Headers["X-SYCA-MERCHANDID"] = request.MerchandId;
                client.Headers["X-SYCA-APIKEY"] = request.ApiKey;
                client.Headers["X-SYCA-REQUEST-DATA-FORMAT"] = request.RequestDataFormat;
                client.Headers["X-SYCA-RESPONSE-DATA-FORMAT"] = request.ResponseDataFormat;
                var data = JsonConvert.SerializeObject(request);
                var objText = client.UploadString("https://secure.sycapay.net/login", data);
                var result = JsonConvert.DeserializeObject<SycapayResponse>(objText);
                return Json(new { txt = result.Token, success = true });
            }

        }

        public ActionResult Index()
        {
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
    }
}