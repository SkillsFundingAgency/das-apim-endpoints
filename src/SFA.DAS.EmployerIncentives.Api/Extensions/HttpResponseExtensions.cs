using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.EmployerIncentives.Infrastructure.Api;

namespace SFA.DAS.EmployerIncentives.Api.Extensions
{
    public static class HttpResponseExtensions
    {
        public static Task WriteJsonAsync(this HttpResponse httpResponse, object body)
        {
            httpResponse.ContentType = "application/json";

            return httpResponse.WriteAsync(JsonConvert.SerializeObject(body, new JsonSerializerSettings
            {
                Converters = new List<JsonConverter> { new StringEnumConverter() },
                Formatting = Formatting.Indented
            }));
        }

        public static ObjectResult CreateObjectResult(this ControllerBase controller, InnerApiResponse innerApiResponse)
        {
            return controller.StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Json?.RootElement);
        }


    }
}
