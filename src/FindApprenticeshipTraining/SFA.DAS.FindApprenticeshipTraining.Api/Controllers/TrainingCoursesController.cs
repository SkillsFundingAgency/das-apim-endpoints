using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SFA.DAS.FindApprenticeshipTraining.Api.ApiRequests;
using SFA.DAS.FindApprenticeshipTraining.Api.Extensions;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourse;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProvider;
using SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.FindApprenticeshipTraining.Api.Controllers
{
    [ApiController]
    [Route("[controller]/")]
    public class TrainingCoursesController : ControllerBase
    {
        private readonly ILogger<TrainingCoursesController> _logger;
        private readonly IMediator _mediator;

        public TrainingCoursesController(
            ILogger<TrainingCoursesController> logger,
            IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, [FromQuery] double lat = 0, [FromQuery] double lon = 0, [FromQuery] string location = "", Guid? shortlistUserId = null)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseQuery
                {
                    Id = id,
                    Lat = lat,
                    Lon = lon,
                    LocationName = location,
                    ShortlistUserId = shortlistUserId
                });

                if (result.Course == null)
                {
                    _logger.LogInformation($"Training course {id} not found");
                    return NotFound();
                }

                var model = new GetTrainingCourseResponse
                {
                    TrainingCourse = result.Course,
                    ProvidersCount = new GetTrainingCourseProviderCountResponse
                    {
                        TotalProviders = result.ProvidersCount,
                        ProvidersAtLocation = result.ProvidersCountAtLocation
                    },
                    ShortlistItemCount = result.ShortlistItemCount
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}/providers")]
        public async Task<IActionResult> GetProviders(int id, [FromQuery] GetCourseProvidersRequest request)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseProvidersQuery
                {
                    Id = id,
                    Location = request.Location,
                    Lat = request.Lat,
                    Lon = request.Lon,
                    ShortlistUserId = request.ShortlistUserId
                });

                var sortedProviders = (request.SortOrder == ProviderCourseSortOrder.SortOrder.Name || request.Lat == 0) ?
                    result.Providers.OrderBy(x => x.Name).ThenByDescending(x => x.DeliveryModels.Any(d => d.LocationType == LocationType.National)) :
                    result.Providers.OrderBy(x => x.DeliveryModelsShortestDistance).ThenByDescending(x => x.DeliveryModels.Any(d => d.LocationType == LocationType.National));

                var mappedProviders = sortedProviders
                    .Select(provider => new GetTrainingCourseProviderListItem().Map(provider, request.DeliveryModes, request.EmployerProviderRatings, request.ApprenticeProviderRatings, result.Location?.GeoPoint != null))
                    .Where(x => x != null)
                    .OrderByProviderScore(request.DeliveryModes)
                    .ToList();
                var model = new GetTrainingCourseProvidersResponse
                {
                    TrainingCourse = result.Course,
                    TrainingCourseProviders = mappedProviders,
                    Total = result.Total,
                    TotalFiltered = mappedProviders.Count,
                    Location = new GetLocationSearchResponseItem
                    {
                        Name = result.Location?.Name,
                        Location = new GetLocationSearchResponseItem.LocationResponse
                        {
                            GeoPoint = result.Location?.GeoPoint
                        }
                    },
                    ShortlistItemCount = result.ShortlistItemCount
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id}");
                return BadRequest();
            }
        }

        [HttpGet]
        [Route("{id}/providers/{providerId}")]
        public async Task<IActionResult> GetProviderCourse(int id, int providerId, [FromQuery] string location, [FromQuery] double lat = 0, [FromQuery] double lon = 0, [FromQuery] Guid? shortlistUserId = null)
        {
            try
            {
                var result = await _mediator.Send(new GetTrainingCourseProviderQuery
                {
                    CourseId = id,
                    ProviderId = providerId,
                    Location = location,
                    Lat = lat,
                    Lon = lon,
                    ShortlistUserId = shortlistUserId
                });

                if (result.ProviderStandard == null)
                {
                    if (result.Course == null)
                    {
                        return NotFound();
                    }
                    return Ok(new GetTrainingCourseProviderResponse
                    {
                        TrainingCourse = result.Course,
                        Location = new GetLocationSearchResponseItem
                        {
                            Name = result.Location?.Name,
                            Location = new GetLocationSearchResponseItem.LocationResponse
                            {
                                GeoPoint = result.Location?.GeoPoint
                            }
                        }
                    });
                }

                var model = new GetTrainingCourseProviderResponse
                {
                    TrainingCourse = result.Course,
                    TrainingCourseProvider = new GetProviderCourseItem().Map(result, result.Course.Level, result.Location?.GeoPoint != null),
                    AdditionalCourses = new GetTrainingAdditionalCourseItem
                    {
                        Total = result.AdditionalCourses.Count(),
                        Courses = result.AdditionalCourses.Select(c => (GetTrainingProviderAdditionalCourseListItem)c).ToList()
                    },
                    ProvidersCount = new GetTrainingCourseProviderCountResponse
                    {
                        TotalProviders = result.TotalProviders,
                        ProvidersAtLocation = result.TotalProvidersAtLocation
                    },
                    Location = new GetLocationSearchResponseItem
                    {
                        Name = result.Location?.Name,
                        Location = new GetLocationSearchResponseItem.LocationResponse
                        {
                            GeoPoint = result.Location?.GeoPoint
                        }
                    },
                    ShortlistItemCount = result.ShortlistItemCount
                };
                return Ok(model);
            }
            catch (Exception e)
            {
                _logger.LogError(e, $"Error attempting to get a training course {id} with provider {providerId}");
                return BadRequest();
            }
        }
    }
}
