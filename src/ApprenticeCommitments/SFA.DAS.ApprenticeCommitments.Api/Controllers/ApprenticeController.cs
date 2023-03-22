using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    public class TemporaryAccountsResponseReturningApiClient : ResponseReturningApiClient
    {
        public TemporaryAccountsResponseReturningApiClient(ApimClient client)
            : base(client)
        {
        }
    }

    [ApiController]
    public class ApprenticeController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public ApprenticeController(TemporaryAccountsResponseReturningApiClient client) => _client = client;

        [HttpGet("/apprentices/{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");


        [HttpPatch("/apprentices/{apprenticeId}")]
        public Task<IActionResult> UpdateApprentice(Guid apprenticeId, [FromBody] JsonPatchDocument<Apprentice> changes)
            => _client.Patch($"apprentices/{apprenticeId}", changes);
    }
}