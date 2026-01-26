using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Apprentices;
using SFA.DAS.ApprenticeCommitments.Application.Queries.Apprenticeships;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;
using System;
using System.Runtime.CompilerServices;
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
    public class ApprenticeController : ApprenticeControllerBase
    {
        private readonly ResponseReturningApiClient _client;
        private readonly IMediator _mediator;

        public ApprenticeController(TemporaryAccountsResponseReturningApiClient client, IMediator mediator) : base(mediator)
        {
            _client = client;
            _mediator = mediator;
        }

        [HttpGet("/apprentices/{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");

        [HttpGet("/apprentices")]
        public async Task<IActionResult> GetApprenticeAccountByName([FromQuery] GetApprenticeByPersonalDetailQuery request)
        {
            var result = await _mediator.Send(request);

            return Ok(result);
        }

        [HttpGet("/apprentice/{uln}")]
        public async Task<IActionResult> GetApprenticeshipByUln([FromRoute] int uln)
        {
            var request = new GetApprenticeshipByUlnQuery { Uln = uln };
            var result = await _mediator.Send(request);
            
            return Ok(result.MyApprenticeship);
        }
    }
}