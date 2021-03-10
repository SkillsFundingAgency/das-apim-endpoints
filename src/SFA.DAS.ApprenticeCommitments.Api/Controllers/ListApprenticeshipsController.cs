using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class ListApprenticeshipsController : ControllerBase
    {
        private readonly IInternalApiClient<ApprenticeCommitmentsConfiguration> _client;

        public ListApprenticeshipsController(IInternalApiClient<ApprenticeCommitmentsConfiguration> client)
        {
            this._client = client;
        }

        [HttpGet("/apprentices/{apprenticeId}/apprenticeships")]
        public async Task<IActionResult> AddApprenticeship(Guid apprenticeId)
        {
            var response = await _client.Get<List<ApprenticeshipsRepsonse>>(
                            new GetApprenticeshipsRequest(apprenticeId));
            if (response == null)
                return NotFound();
            else
                return Ok(response);
        }
    }
}