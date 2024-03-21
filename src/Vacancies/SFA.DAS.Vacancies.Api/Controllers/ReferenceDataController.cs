using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.Vacancies.Api.Models;
using SFA.DAS.Vacancies.Application.TrainingCourses.Queries;

namespace SFA.DAS.Vacancies.Api.Controllers
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
        /// GET list of courses. 
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

        /// <summary>
        /// GET list of course routes. The routes can then be used to filter results in `GET Vacancy` endpoint.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("courses/routes")]
        [ProducesResponseType(typeof(GetRouteResponseItem), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRoutes()
        {
            try
            {
                var response = await _mediator.Send(new GetRoutesQuery());
                return Ok(new GetRoutesResponse
                {
                    Routes = response.Routes.Select(c=>(GetRouteResponseItem)c).ToList()
                });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error attempting to get course routes");
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
        }
    }
}