using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    [Route("/apprentices")]
    public class ApprenticeController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public ApprenticeController(ResponseReturningApiClient client) => _client = client;

        [HttpPost]
        public Task<IActionResult> CreateApprentice(Apprentice apprentice)
            => _client.Post("apprentices", apprentice);

        [HttpGet("{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");

        [HttpGet("/apprentices/{apprenticeId}/apprenticeships")]
        public Task<IActionResult> ListApprenticeApprenticeships(Guid id)
            => _client.Get($"apprentices/{id}");

        [HttpPatch("{id}")]
        public Task<IActionResult> UpdateApprentice(Guid id, JsonPatchDocument<Apprentice> changes)
            => _client.Patch($"apprentices/{id}", changes);

        [HttpPost("{id}/email")]
        public Task<IActionResult> ChangeApprenticeEmailAddress(Guid id, ApprenticeEmailAddressRequest request)
            => _client.Post($"/apprentices/{id}/email", request);
    }
}