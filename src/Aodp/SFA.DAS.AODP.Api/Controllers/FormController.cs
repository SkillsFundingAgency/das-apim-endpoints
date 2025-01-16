using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Form;
using SFA.DAS.AODP.InnerApi.AodpApi.Responses;

namespace SFA.DAS.AODP.Api.AppStart
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : Controller
    {
        private readonly IMediator _mediator;

        public FormController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AllowAnonymous]
        [HttpGet("/api/forms")]
        public async Task<IActionResult> GetFormsAsync()
        {
            var response = await _mediator.Send(new GetAllFormsQuery());
            if (response.Success)
            {
                var apiResponse = new GetAllFormsResponse() { Forms = new() };

                foreach (var form in response.Forms)
                {
                    apiResponse.Forms.Add(new Form()
                    {
                        Id = form.Id,
                        Description = form.Description,
                        Name = form.Name,
                        Order = form.Order,
                        Published = form.Published,
                        Version = form.Version
                    });
                }

                return Ok(apiResponse);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }
    }
}
