using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.AODP.Application.Commands.FormBuilder.Forms;
using SFA.DAS.AODP.Application.Queries.FormBuilder.Forms;
using SFA.DAS.AODP.Domain.FormBuilder.Responses.Forms;
using SFA.DAS.AODP.InnerApi.AodpApi.Responses;

namespace SFA.DAS.AODP.Api.AppStart
{
    [ApiController]
    [Route("api/[controller]")]
    public class FormController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public FormController(IMediator mediator, IMapper mapper)
        {
            _mediator = mediator;
            _mapper = mapper;
        }

        [AllowAnonymous]
        [HttpGet("/api/forms")]
        public async Task<IActionResult> GetFormsAsync()
        {
            var response = await _mediator.Send(new GetAllFormVersionsQuery());
            if (response.Success)
            {
                var result = _mapper.Map<GetAllFormVersionsApiResponse>(response);
                //var apiResponse = new GetAllFormVersionsApiResponse() { Forms = new() };

                //foreach (var form in response.Data)
                //{
                //    apiResponse.Data.Add(new()
                //    {
                //        Id = form.Id,
                //        Description = form.Description,
                //        Name = form.Name,
                //        Order = form.Order,
                //        //Published = form.Published,
                //        //Version = form.Version
                //    });
                //}

                return Ok(result);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }

        //#################
        [HttpGet("/api/forms")]
        [ProducesResponseType(typeof(List<GetAllFormVersionsApiResponse.FormVersion>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync()
        {
            var query = new GetAllFormVersionsQuery();
            var response = await _mediator.Send(query);
            if (response.Success)
            {
                var result = _mapper.Map<GetAllFormVersionsApiResponse>(response);
                return Ok(result.Data);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }

        [HttpGet("/api/forms/{formVersionId}")]
        [ProducesResponseType(typeof(GetFormVersionByIdApiResponse.FormVersion), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetByIdAsync(Guid formVersionId)
        {
            var request = new GetFormVersionByIdQuery(formVersionId);

            var response = await _mediator.Send(request);

            if (response.Success)
            {
                var result = _mapper.Map<GetFormVersionByIdApiResponse>(response);
                if (result.Data is null)
                    return NotFound();


                return Ok(result.Data);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }

        [HttpPost("/api/forms")]
        [ProducesResponseType(typeof(CreateFormVersionApiResponse.FormVersion), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateFormVersionCommand.FormVersion formVersion)
        {
            var command = new CreateFormVersionCommand(formVersion);

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                var result = _mapper.Map<CreateFormVersionApiResponse>(response);
                if (result.Data is null)
                    return NotFound();
                return Ok(result.Data);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }

        [HttpPut("/api/forms/{formVersionId}")]
        [ProducesResponseType(typeof(UpdateFormVersionApiResponse.FormVersion), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateAsync(Guid formVersionId, [FromBody] UpdateFormVersionCommand.FormVersion formVersion)
        {
            var command = new UpdateFormVersionCommand(formVersionId, formVersion);

            var response = await _mediator.Send(command);

            if (response.Success)
            {
                var result = _mapper.Map<UpdateFormVersionApiResponse>(response);
                if (result.Data is null)
                    return NotFound();
                return Ok(result.Data);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }

        [HttpPut("/api/forms/{formVersionId}/publish")]
        public async Task<IActionResult> PublishAsync(Guid formVersionId)
        {
            return Ok();
        }

        [HttpPut("/api/forms/{formVersionId}/unpublish")]
        public async Task<IActionResult> UnpublishAsync(Guid formVersionId)
        {
            return Ok();
        }

        [HttpDelete("/api/forms/{formVersionId}")]
        [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveAsync(Guid formVersionId)
        {
            var command = new DeleteFormVersionCommand(formVersionId);

            var response = await _mediator.Send(command);
            if (response.Success)
            {
                var result = new DeleteFormVersionApiResponse();
                result.Data = response.Data;
                return Ok(result.Data);
            }

            var errorObjectResult = new ObjectResult(response.ErrorMessage);
            errorObjectResult.StatusCode = StatusCodes.Status500InternalServerError;

            return errorObjectResult;
        }
    }
}
