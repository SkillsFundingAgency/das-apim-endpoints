using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Campaign.Api.Models;
using SFA.DAS.Campaign.Application.Queries.Panel;
using SFA.DAS.Campaign.Application.Queries.PreviewPanel;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Campaign.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class PanelController : Controller
    {
        private readonly IMediator mediator;

        public PanelController(IMediator _mediator)
        {
            mediator = _mediator;
        }

        [HttpGet]
        [Route("{panelId}")]

        public async Task<IActionResult> GetPanelAsync(int panelId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPanelQuery
            {
                Id = panelId
            }, cancellationToken);

            if (result.Panel == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Panel not found for panel id {panelId}."
                });
            }

            return Ok(new GetPanelResponse
            {
                Panel = result.Panel
            });

        }

        [HttpGet]
        [Route("preview/{panelId}")]
        public async Task<IActionResult> GetPreviewPanelAsync(int panelId, CancellationToken cancellationToken)
        {
            var result = await mediator.Send(new GetPreviewPanelQuery
            {
                Id = panelId
            }, cancellationToken);

            if (result == null)
            {
                return new NotFoundObjectResult(new NotFoundResponse
                {
                    Message = $"Preview Panel not found for panel id {panelId}."
                });
            }

            return Ok(new GetPreviewPanelResponse
            {
                Panel = result.PanelModel
            });

        }
    }
}
