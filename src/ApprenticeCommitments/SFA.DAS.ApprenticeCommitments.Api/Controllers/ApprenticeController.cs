using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;

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
    public class ApprenticeController : ApprenticeControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public ApprenticeController(TemporaryAccountsResponseReturningApiClient client, IMediator mediator) : base(mediator) => _client = client;

        [HttpGet("/apprentices/{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");
    }
}