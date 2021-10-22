using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class CandidateSkillsController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<CandidateSkillsController> _logger;

        public CandidateSkillsController (IMediator mediator, ILogger<CandidateSkillsController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> GetList()
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetCandidateSkillsQuery());

                return Ok((GetCandidateSkillsListResponse) queryResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get candidate skills");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}