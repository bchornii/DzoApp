using System;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Application.Web.Custom
{
    public class JsonController : Controller
    {
        private const string ContentType = "application/json";
        protected new ActionResult Json(object data, JsonRequestBehavior behavior = JsonRequestBehavior.AllowGet)
        {
            if (Request.RequestType == WebRequestMethods.Http.Get
                && behavior == JsonRequestBehavior.DenyGet)
                throw new Exception("Get is not allowed");
            var serializationSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            var jsonResult = new ContentResult
            {
                ContentType = ContentType,
                Content = JsonConvert.SerializeObject(data, serializationSettings)
            };
            return jsonResult;
        }
    }
}