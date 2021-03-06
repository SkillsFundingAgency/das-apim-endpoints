using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [Controller]
    public class QueryForwardingController : ControllerBase
    {
        private readonly GenericInnerApiQueryClient client;

        public QueryForwardingController(GenericInnerApiQueryClient client)
        {
            this.client = client;
        }

        [HttpGet]
        public async Task<IActionResult> ForwardToInnerApi()
        {
            var innerResponse = await client.GetAsync(HttpContext.Request.Path);

            if (!innerResponse.IsSuccessStatusCode)
                return NotFound();

            var content = await innerResponse.Content.ReadAsStringAsync();

            return new ContentResult
            {
                Content = content,
                ContentType = "application/json",
                StatusCode = 200
            };
        }
    }
}