using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.JsonPatch.Operations;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.ApprenticeAccountsApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using SFA.DAS.SharedOuterApi.Apprentice.GovUK.Auth.Controllers;
using System;
using System.Text.RegularExpressions;
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
        private readonly UpdateApprenticeValidator _validator = new UpdateApprenticeValidator();

        public ApprenticeController(TemporaryAccountsResponseReturningApiClient client, IMediator mediator) : base(mediator) => _client = client;

        [HttpGet("/apprentices/{id}")]
        public Task<IActionResult> GetApprentice(Guid id)
            => _client.Get($"apprentices/{id}");


        [HttpPatch("/apprentices/{id}")]
        [Consumes("application/json", "application/json-patch+json", "text/json", "application/*+json")]
        public async Task<IActionResult> UpdateApprentice(Guid id, JsonPatchDocument<Apprentice> patch)
        {
            if (patch == null)
                return BadRequest("Patch document is required.");

            // Validate each operation in the patch document
            foreach (var operation in patch.Operations)
            {
                if (!_validator.IsValidPatchOperation(operation))
                {
                    return BadRequest($"Invalid value for path '{operation.path}'. " +
                                      "Only letters, spaces, hyphens, and apostrophes are allowed for name fields.");
                }
            }

            // All validations passed — forward to the underlying API client
            return await _client.Patch($"apprentices/{id}", patch);
        }        
    }
}