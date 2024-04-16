using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFeedback.Api.Models;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;

namespace SFA.DAS.ProviderFeedback.Api.Controllers
{
    [ApiController]
    [Route("/providerfeedback/")]
    public class ProviderFeedbackController : ControllerBase
    {
        private readonly ILogger<ProviderFeedbackController> _logger;
        private readonly IMediator _mediator;

        public ProviderFeedbackController(
            ILogger<ProviderFeedbackController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("provider/{providerId}")]
        public async Task<IActionResult> GetProviderFeedback(int providerId)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderFeedbackQuery
                {
                    ProviderId = providerId,
                });

                if (result.ProviderStandard == null)
                {
                    return NotFound();
                }

                var model = new GetProviderFeedbackResponse
                {
                    ProviderFeedback = new GetProviderFeedbackItem().Map(result),
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get provider feedback {providerId}");
                return BadRequest();
            }
        }
    }
}
