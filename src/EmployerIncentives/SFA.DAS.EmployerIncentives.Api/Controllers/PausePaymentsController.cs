using System.Net;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EmployerIncentives.InnerApi.Requests;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Commands.PausePayments;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerIncentives.Api.Controllers
{
    [ApiController]
    public class PausePaymentsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PausePaymentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("/pause-payments")]
        public async Task<IActionResult> PausePayments([FromBody] PausePaymentsRequest request)
        {
            try
            {
                await _mediator.Send(new PausePaymentsCommand(request), CancellationToken.None);

                return Ok();
            }
            catch (HttpRequestContentException e)
            {
                var response = BuildErrorResponse(e);
                if (response != null)
                {
                    return response;
                }
                throw;
            }
        }

        private IActionResult BuildErrorResponse(HttpRequestContentException e)
        {
            SingleMessageResponse response = null;
            if (!string.IsNullOrWhiteSpace(e.ErrorContent))
            {
                response = JsonConvert.DeserializeObject<SingleMessageResponse>(e.ErrorContent);
            }

            switch (e.StatusCode)
            {
                case HttpStatusCode.NotFound:
                    return new NotFoundObjectResult(response);
                case HttpStatusCode.BadRequest:
                    return new BadRequestObjectResult(response);
            }

            return null;
        }
    }
}
