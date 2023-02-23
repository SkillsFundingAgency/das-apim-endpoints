using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;
using SFA.DAS.FindApprenticeshipTraining.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindApprenticeshipTraining.Configuration;

namespace SFA.DAS.FindApprenticeshipTraining.Application.TrainingCourses.Queries.GetTrainingCourseProviders
{
    public class GetTrainingCourseProvidersQueryHandler : IRequestHandler<GetTrainingCourseProvidersQuery, GetTrainingCourseProvidersResult>
    {
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        private readonly IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> _apprenticeFeedbackApiClient;
        private readonly IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> _employerFeedbackApiClient;
        private readonly ICoursesApiClient<CoursesApiConfiguration> _coursesApiClient;
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        private readonly ILocationLookupService _locationLookupService;

       
        public GetTrainingCourseProvidersQueryHandler(
            IApprenticeFeedbackApiClient<ApprenticeFeedbackApiConfiguration> apprenticeFeedbackApiClient,
            IEmployerFeedbackApiClient<EmployerFeedbackApiConfiguration> employerFeedbackApiClient,
            ICoursesApiClient<CoursesApiConfiguration> coursesApiClient,
            ILocationLookupService locationLookupService, 
            IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient, 
            IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _apprenticeFeedbackApiClient = apprenticeFeedbackApiClient;
            _employerFeedbackApiClient = employerFeedbackApiClient;
            _coursesApiClient = coursesApiClient;
            _locationLookupService = locationLookupService;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
            _shortlistApiClient = shortlistApiClient;
        }

        public async Task<GetTrainingCourseProvidersResult> Handle(GetTrainingCourseProvidersQuery request, CancellationToken cancellationToken)
        {
            var locationTask = _locationLookupService.GetLocationInformation(request.Location, request.Lat, request.Lon);

            var courseTask = _coursesApiClient.Get<GetStandardsListItem>(new GetStandardRequest(request.Id));

            var shortlistTask = request.ShortlistUserId.HasValue
                ? _shortlistApiClient.GetAll<ShortlistItem>(
                    new GetShortlistForUserIdRequest(request.ShortlistUserId.Value))
                : Task.FromResult<IEnumerable<ShortlistItem>>(new List<ShortlistItem>());

            var apprenticeFeedbackSummaryTask = _apprenticeFeedbackApiClient.GetAll<GetApprenticeFeedbackSummaryItem>(new GetApprenticeFeedbackSummaryRequest());

            var employerFeedbackSummaryTask = _employerFeedbackApiClient.GetAll<GetEmployerFeedbackSummaryItem>(new GetEmployerFeedbackSummaryRequest());

            await Task.WhenAll(locationTask, courseTask, shortlistTask, apprenticeFeedbackSummaryTask, employerFeedbackSummaryTask);

            var providers = await _roatpCourseManagementApiClient.Get<GetProvidersListFromCourseIdResponse> (new GetProvidersByCourseIdRequest(
                request.Id, locationTask.Result?.GeoPoint?.FirstOrDefault(),
                locationTask.Result?.GeoPoint?.LastOrDefault()));

            var filteredProviders = providers?.Providers.Where(x => x.IsApprovedByRegulator != false).ToList();

            if (filteredProviders?.Any() == true && apprenticeFeedbackSummaryTask.Result?.Any() == true)
            {
                var summaries = apprenticeFeedbackSummaryTask.Result;
                foreach (var provider in filteredProviders)
                {
                    provider.ApprenticeFeedback = summaries.FirstOrDefault(s => s.Ukprn == provider.Ukprn);
                }
            }

            if (filteredProviders?.Any() == true && employerFeedbackSummaryTask.Result?.Any() == true)
            {
                var summaries = employerFeedbackSummaryTask.Result;
                foreach (var provider in filteredProviders)
                {
                    provider.EmployerFeedback = summaries.FirstOrDefault(s => s.Ukprn == provider.Ukprn);
                }
            }

            if (filteredProviders?.Any() == true && shortlistTask.Result?.Any() == true)
            {
                var shortlistItems = shortlistTask.Result;
                foreach (var provider in filteredProviders)
                {
                    provider.ShortlistId = shortlistItems.FirstOrDefault(s => s.Ukprn == provider.Ukprn && s.Larscode == request.Id 
                        && s.LocationDescription == request.Location)?.Id;
                }
            }

            return new GetTrainingCourseProvidersResult
            {
                Course = courseTask.Result,
                Providers = filteredProviders,
                Total = filteredProviders?.Count() ?? 0,
                Location = locationTask.Result,
                ShortlistItemCount = shortlistTask?.Result?.Count() ?? 0
            };
        }
    }
}