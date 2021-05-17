using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.EmployerDemand.Api.ApiRequests;
using SFA.DAS.EmployerDemand.Api.Models;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.RegisterDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.VerifyEmployerDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetAggregatedCourseDemandList;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetEmployerCourseProviderDemand;
using SFA.DAS.EmployerDemand.Application.Demand.Queries.GetRegisterDemand;
using SFA.DAS.SharedOuterApi.Infrastructure;

namespace SFA.DAS.EmployerDemand.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class DemandController : ControllerBase
    {
        private readonly ILogger<DemandController> _logger;
        private readonly IMediator _mediator;

        public DemandController(
            ILogger<DemandController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("create/{courseId}")]
        public async Task<IActionResult> Create(int courseId, [FromQuery]string location)
        {
            try
            {
                var queryResult = await _mediator.Send(new GetRegisterDemandQuery
                {
                    CourseId = courseId,
                    LocationName = location
                });
                
                var model = new GetCourseResponse
                {
                    TrainingCourse = queryResult.Course,
                    Location = queryResult?.Location
                };

                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get create course demand for course id [{courseId}] and location:{location}");
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateCourseDemand(CreateCourseDemandRequest request)
        {
            try
            {
                var commandResult = await _mediator.Send(new RegisterDemandCommand
                {
                    Id = request.Id,
                    OrganisationName = request.OrganisationName,
                    ContactEmailAddress = request.ContactEmailAddress,
                    NumberOfApprentices = request.NumberOfApprentices,
                    Lat = request.LocationItem.Location.GeoPoint.First(),
                    Lon = request.LocationItem.Location.GeoPoint.Last(),
                    LocationName = request.LocationItem.Name,
                    CourseId = request.TrainingCourse.Id,
                    CourseTitle = request.TrainingCourse.Title,
                    CourseLevel = request.TrainingCourse.Level,
                    CourseRoute = request.TrainingCourse.Route,
                    ConfirmationLink = request.ResponseUrl
                });

                return Created("", commandResult);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating course demand item");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("verify/{id}")]
        public async Task<IActionResult> VerifyCourseDemand(Guid id)
        {
            try
            {
                var commandResult = await _mediator.Send(new VerifyEmployerDemandCommand
                {
                    Id = id
                });
                var model =  (VerifyCourseDemandResponse) commandResult;

                return Created("", model);
            }
            catch (HttpRequestContentException e)
            {
                return StatusCode((int) e.StatusCode, e.ErrorContent);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error creating course demand item");
                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
            
        }

        [HttpGet]
        [Route("aggregated/providers/{ukprn}")]
        public async Task<IActionResult> GetAggregatedCourseDemandList([FromRoute]int ukprn, int? courseId, string location, int? locationRadius, [FromQuery]List<string> routes)
        {
            try
            {
                var result = await _mediator.Send(new GetAggregatedCourseDemandListQuery
                {
                    Ukprn = ukprn,
                    CourseId = courseId,
                    LocationName = location,
                    LocationRadius = locationRadius,
                    Routes = routes
                });

                var apiResponse = new GetAggregatedCourseDemandListResponse
                {
                    TrainingCourses = result.Courses.Select(item => (GetCourseListItem)item),
                    AggregatedCourseDemands = result.AggregatedCourseDemands.Select(response => (GetAggregatedCourseDemandSummary)response),
                    Total = result.Total,
                    TotalFiltered = result.TotalFiltered,
                    Location = result.LocationItem,
                    Routes = result.Routes.Select(c => (GetRoutesListItem)c).ToList()
                };

                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error getting aggregated demand list for ukprn:[{ukprn}], courseId:[{courseId}], location:[{location}]");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("providers/{ukprn}/courses/{courseId}")]
        public async Task<IActionResult> GetEmployerCourseProviderDemand([FromRoute] int ukprn, [FromRoute]int courseId,
            string location, int? locationRadius)
        {
            try
            {
                var result = await _mediator.Send(new GetEmployerCourseProviderDemandQuery
                {
                    Ukprn = ukprn,
                    CourseId = courseId,
                    LocationName = location,
                    LocationRadius = locationRadius
                });

                var apiResponse = new GetProviderEmployerCourseDemandListResponse
                {
                    ProviderEmployerDemandDetailsList = result.EmployerCourseDemands.Select(c=>(GetProviderEmployerDemandDetailsListItem)c),
                    TrainingCourse = result.Course,
                    Total = result.Total,
                    TotalFiltered = result.TotalFiltered,
                    Location = result.Location,
                    ProviderContactDetails = result.ProviderDetail
                };
                
                return Ok(apiResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return new StatusCodeResult((int) HttpStatusCode.InternalServerError);
            }
            
        }
    }
}
