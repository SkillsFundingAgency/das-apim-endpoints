using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Extensions
{
    public static class HttpResponseExtensions
    {
        public static ObjectResult CreateObjectResult(this ControllerBase controller, InnerApiResponse innerApiResponse)
        {
            return controller.StatusCode((int)innerApiResponse.StatusCode, innerApiResponse.Json?.RootElement);
        }
    }
}
