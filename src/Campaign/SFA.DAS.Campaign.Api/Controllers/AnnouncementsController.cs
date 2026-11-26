using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Announcement;
using SFA.DAS.Campaign.Application.Queries.Announcements;
using SFA.DAS.Campaign.Application.Queries.PreviewAnnouncement;
using SFA.DAS.Campaign.Application.Queries.PreviewAnnouncements;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AnnouncementsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AnnouncementsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnnouncementsAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAnnouncementsQuery(), cancellationToken);

            return Ok(new GetAnnouncementsResponse
            {
                Announcements = result.PageModel
            });
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetAnnouncementAsync(string slug, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetAnnouncementBySlugQuery { Slug = slug }, cancellationToken);

            if (result?.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Announcement not found for slug {slug}."
                });
            }

            return Ok(new GetAnnouncementResponse
            {
                Announcement = result.PageModel
            });
        }

        [HttpGet("preview")]
        public async Task<IActionResult> GetPreviewAnnouncementsAsync(CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPreviewAnnouncementsQuery(), cancellationToken);

            return Ok(new GetAnnouncementsResponse
            {
                Announcements = result?.PageModel
            });
        }

        [HttpGet("preview/{slug}")]
        public async Task<IActionResult> GetPreviewAnnouncementAsync(string slug, CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetPreviewAnnouncementBySlugQuery { Slug = slug }, cancellationToken);

            if (result?.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview announcement not found for slug {slug}."
                });
            }

            return Ok(new GetAnnouncementResponse
            {
                Announcement = result.PageModel
            });
        }
    }
}
