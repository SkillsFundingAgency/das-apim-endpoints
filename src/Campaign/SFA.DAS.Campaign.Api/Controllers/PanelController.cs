using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Panel;
using SFA.DAS.Campaign.Application.Queries.PreviewPanels;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class PanelController: Controller
    {
        private readonly IMediator mediator;

        public PanelController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet]
        [Route("{title}")]

        public async Task<IActionResult> GetPanelAsync(string title, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPanelQuery
            {
                Title = title
            }, cancellationToken);

            if (result == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Panel not found for {title}"
                });
            }

            return Ok(new GetPanelResponse
            {
                PanelModel = result.Panel
            });

        }

        [HttpGet]
        [Route("preview/{title}")]
        public async Task<IActionResult> GetPreviewPanelAsync(string title, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPreviewPanelQuery
            {
                Title = title
            }, cancellationToken);

            if (result == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview Panel not found for {title}"
                });
            }

            return Ok(new GetPreviewPanelResponse
            {
                PanelModel = result.PanelModel
            });

        }
    }
}
