using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ApprenticeApp.Application.Commands.LearnerNotifications;
using SFA.DAS.ApprenticeApp.Application.Queries.LearnerNotifications;
using SFA.DAS.ApprenticeApp.Models;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;


namespace SFA.DAS.ApprenticeApp.Api.Controllers
{
    [ApiController]
    [Route("api/learner/{accountIdentifier}/notifications")]
    public class LearnerNotificationsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public LearnerNotificationsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetLearnerNotifications(
            Guid accountIdentifier,
            [FromQuery] string order = null,
            [FromQuery] DateTime? dateFrom = null,
            [FromQuery] List<int> statuses = null)
        {
            var result = await _mediator.Send(new GetLearnerNotificationsQuery 
            { 
                AccountIdentifier = accountIdentifier,
                Order = order,
                DateFrom = dateFrom,
                Statuses = statuses
            });

            if (result?.Notifications == null)
                return NotFound();

            return Ok(result.Notifications);
        }

        [HttpGet("{notificationIdentifier}")]
        public async Task<IActionResult> GetLearnerNotificationById(Guid accountIdentifier, long notificationIdentifier)
        {
            var result = await _mediator.Send(new GetLearnerNotificationByIdQuery 
            { 
                AccountIdentifier = accountIdentifier,
                NotificationIdentifier = notificationIdentifier
            });

            if (result?.Notification == null)
                return NotFound();

            return Ok(result.Notification);
        }

        [HttpGet("{notificationIdentifier}/status")]
        public async Task<IActionResult> GetLearnerNotificationStatus(Guid accountIdentifier, long notificationIdentifier)
        {
            var result = await _mediator.Send(new GetLearnerNotificationStatusQuery 
            { 
                AccountIdentifier = accountIdentifier,
                NotificationIdentifier = notificationIdentifier
            });

            if (result?.NotificationStatus == null)
                return NotFound();

            return Ok(result.NotificationStatus);
        }

        [HttpPut("{notificationIdentifier}/status")]
        public async Task<IActionResult> UpdateLearnerNotificationStatus(
            Guid accountIdentifier, 
            long notificationIdentifier, 
            [FromBody] UpdateNotificationStatusRequest request)
        {
            await _mediator.Send(new UpdateLearnerNotificationStatusCommand 
            { 
                AccountIdentifier = accountIdentifier,
                NotificationIdentifier = notificationIdentifier,
                StatusId = request.StatusId
            });

            return Ok();
            
        }

        [HttpDelete("{notificationIdentifier}")]
        public async Task<IActionResult> DeleteLearnerNotification(
            Guid accountIdentifier,
            long notificationIdentifier)
        {
            await _mediator.Send(new DeleteLearnerNotificationCommand
            {
                AccountIdentifier = accountIdentifier,
                NotificationIdentifier = notificationIdentifier
            });

            return NoContent();
        }
    }
}