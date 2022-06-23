using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.VacanciesManage.Api.Models;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetCandidateSkills;
using SFA.DAS.VacanciesManage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.VacanciesManage.Application.TrainingCourses.Queries;

namespace SFA.DAS.VacanciesManage.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class ReferenceDataController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<ReferenceDataController> _logger;

        public ReferenceDataController (IMediator mediator, ILogger<ReferenceDataController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }
        
        /// <summary>
        /// GET list of qualifications.
        /// </summary>
        /// <remarks>Returns list of qualifications to be used when creating a Vacancy.</remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("qualifications")]
        [ProducesResponseType(typeof(GetQualificationsResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetQualifications()
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetQualificationsQuery());

                return Ok((GetQualificationsResponse) queryResponse);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get qualifications");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
        
        /// <summary>
        /// GET list of candidate skills. 
        /// </summary>
        /// <remarks>
        /// Returns list of candidate skills to be used when creating a Vacancy.
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("skills")]
        [ProducesResponseType(typeof(GetCandidateSkillsListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetSkills()
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

        /// <summary>
        /// GET list of courses.
        /// </summary>
        /// <remarks>
        /// Returns list of courses to be used when creating a Vacancy. The `Id` should be used for `standardsLarsCode` in create Vacancy
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("courses")]
        [ProducesResponseType(typeof(GetTrainingCoursesListResponse), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetTrainingCourses()
        {
            try
            {
                var queryResponse = await _mediator.Send(new GetTrainingCoursesQuery());

                return Ok((GetTrainingCoursesListResponse) queryResponse);

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get training courses");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}