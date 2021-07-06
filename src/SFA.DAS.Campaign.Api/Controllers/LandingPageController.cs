using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.LandingPage;
using SFA.DAS.Campaign.Application.Queries.PreviewLandingPage;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class LandingPageController : Controller
    {
        private readonly IMediator _mediator;


        public LandingPageController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{hub}/{slug}")]
        public async Task<IActionResult> GetLandingPageAsync(string hub, string slug,
            CancellationToken cancellationToken = default)
        {
            var result = await _mediator.Send(new GetLandingPageQuery
            {
                Hub = hub,
                Slug = slug
            }, cancellationToken);

            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Landing page not found for {hub}/{slug}"
                });
            }    

            return Ok(new GetLandingPageResponse
            {
                LandingPage = result.PageModel
            });

        }

        [HttpGet("preview/{hub}/{slug}")]
        public async Task<IActionResult> GetPreviewLandingPage(string hub, string slug)
        {
            var result = await _mediator.Send(new GetPreviewLandingPageQuery
            {
                Hub = hub,
                Slug = slug
            });
            if (result.PageModel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview landing page not found for {hub}/{slug}"
                });
            }
            return Ok(new GetLandingPageResponse()
            {
                LandingPage= result.PageModel
            });
        }
    }
}