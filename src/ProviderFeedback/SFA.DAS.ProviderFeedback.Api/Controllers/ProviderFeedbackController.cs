using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.ProviderFeedback.Api.Models;
using SFA.DAS.ProviderFeedback.Application.Queries.GetProviderFeedback;

namespace SFA.DAS.ProviderFeedback.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
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
        [Route("{ukprn}")]
        public async Task<IActionResult> GetProviderFeedback(int ukprn)
        {
            try
            {
                var result = await _mediator.Send(new GetProviderFeedbackQuery
                {
                    ProviderId = ukprn,
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
                _logger.LogError(e, $"Error attempting to get provider feedback for {ukprn}");
                return BadRequest();
            }
        }
    }
}
