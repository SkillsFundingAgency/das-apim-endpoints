﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.EarlyConnect.Api.Models;
using System.Net;
using SFA.DAS.EarlyConnect.Application.Commands.CreateOtherStudentTriageData;
using SFA.DAS.EarlyConnect.InnerApi.Requests;
using SFA.DAS.EarlyConnect.Application.Queries.GetStudentTriageDataBySurveyId;

namespace SFA.DAS.EarlyConnect.Api.Controllers
{
    [ApiVersion("1.0")]
    [ApiController]
    [Route("/early-connect/student-triage-data/")]
    public class StudentTriageDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<StudentTriageDataController> _logger;
        private readonly string DataSource = "UCAS";

        public StudentTriageDataController(IMediator mediator, ILogger<StudentTriageDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("survey-create")]
        public async Task<IActionResult> CreateStudentTriageData(CreateOtherStudentTriageDataPostRequest request)
        {
            try
            {
                var response = await _mediator.Send(new CreateOtherStudentTriageDataCommand
                {
                    StudentTriageData = new OtherStudentTriageData
                    {
                        Email = request.Email,
                        LepsCode = request.LepsCode
                    }
                });

                return CreatedAtAction(nameof(CreateStudentTriageData), (CreateOtherStudentTriageDataPostResponse)response);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error posting student triage data");

                return BadRequest();
            }
        }

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [Route("{surveyGuid}")]
        public async Task<IActionResult> GetStudentTriageData([FromRoute] string surveyGuid)
        {
            try
            {
                var result = await _mediator.Send(new GetStudentTriageDataBySurveyIdQuery { SurveyGuid = surveyGuid });

                return Ok((GetStudentTriageDataBySurveyIdResponse)result);
            }
            catch (Exception e)
            {
                var errorMessage = (e as SharedOuterApi.Exceptions.ApiResponseException)?.Error;

                _logger.LogError(e, "Error getting student triage data ");

                return BadRequest($"Error getting student triage data. {(errorMessage != null ? $"\nErrorInfo: {errorMessage}" : "")}");
            }
        }
    }
}
