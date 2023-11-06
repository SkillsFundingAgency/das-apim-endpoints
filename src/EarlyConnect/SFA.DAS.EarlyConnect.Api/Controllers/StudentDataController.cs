using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Mappers;
using SFA.DAS.EarlyConnect.Api.Models;
using SFA.DAS.EarlyConnect.Application.Commands.StudentData;
using System.Net;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/api/student-data/")]
    public class StudentDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentDataController> _logger;

        public StudentDataController(IMediator mediator, ILogger<StudentDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [Route("")]
        public async Task<IActionResult> Post(CreateStudentDataPostRequest request)
        {
            try
            {
                await _mediator.Send(new CreateStudentDataCommand
                {
                    StudentDataList = request.MapFromCreateStudentDataRequest()
                });

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student data");
                return BadRequest();
            }
        }
    }
}
