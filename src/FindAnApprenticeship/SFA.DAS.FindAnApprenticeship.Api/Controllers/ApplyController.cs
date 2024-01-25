using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SFA.DAS.FindAnApprenticeship.Api.Controllers
{
    [ApiController]
    [Route("vacancies/{vacancyReference}/[controller]")]
    public class ApplyController : Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ApplyController> _logger;

        public ApplyController(IMediator mediator, ILogger<ApplyController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
    }
}
