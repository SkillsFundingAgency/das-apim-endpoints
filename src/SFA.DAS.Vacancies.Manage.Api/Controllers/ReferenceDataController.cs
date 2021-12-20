﻿using System;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Manage.Api.Models;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetCandidateSkills;
using SFA.DAS.Vacancies.Manage.Application.Recruit.Queries.GetQualifications;
using SFA.DAS.Vacancies.Manage.Application.TrainingCourses.Queries;

namespace SFA.DAS.Vacancies.Manage.Api.Controllers
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
        /// Returns list of qualifications to be used when creating a Vacancy.
        /// </summary>
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
        /// Returns list of candidate skills to be used when creating a Vacancy. 
        /// </summary>
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
        ///  Returns list of courses to be used when creating a Vacancy. The `Id` should be used for `standardsLarsCode` in create Vacancy
        /// </summary>
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