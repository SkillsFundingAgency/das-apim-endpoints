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

        [HttpPost("/apprentices")]
        public Task<IActionResult> CreateApprentice(Apprentice apprentice)
            => _client.Post("apprentices", apprentice);

        [HttpGet("/apprentices/{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");
    }
}