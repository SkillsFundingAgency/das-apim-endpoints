using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeCommitments.Apis.InnerApi;
using SFA.DAS.ApprenticeCommitments.Application.Services;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeCommitments.Api.Controllers
{
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly ResponseReturningApiClient _client;

        public RegistrationController(ResponseReturningApiClient client)
            => _client = client;

        [HttpGet]
        [Route("/registrations/{registrationId}")]
        public Task<IActionResult> Get(Guid registrationId)
            => _client.Get($"registrations/{registrationId}");

        [HttpGet]
        [Route("/registrations/reminders")]
        public Task<IActionResult> GetRemindersToSend([FromQuery] DateTime invitationCutOffTime)
            => _client.Get($"registrations/reminders?invitationCutOffTime={invitationCutOffTime}");

        [HttpPost("/registrations/{registrationId}/reminder")]
        public Task<IActionResult> RegistrationReminderSent(Guid registrationId, [FromBody] InvitationReminderSentData request)
            => _client.Post($"registrations/{registrationId}/reminder", request);

        [HttpPost("/registrations/{registrationId}/firstseen")]
        public Task<IActionResult> RegistrationFirstSeen(Guid registrationId, [FromBody] RegistrationFirstSeenRequestData request)
            => _client.Post($"registrations/{registrationId}/firstseen", request);
    }
}