using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Enums;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.GetReviewApprenticeshipUpdates
{
    public class GetReviewApprenticeshipUpdatesQueryHandler : IRequestHandler<GetReviewApprenticeshipUpdatesQuery, GetReviewApprenticeshipUpdatesQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly IProviderStandardsService _providerStandardsService;
        private readonly ServiceParameters _serviceParameters;

        public GetReviewApprenticeshipUpdatesQueryHandler(
            ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
            IProviderStandardsService providerStandardsService,
            ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _providerStandardsService = providerStandardsService;
            _serviceParameters = serviceParameters;
        }

        public async Task<GetReviewApprenticeshipUpdatesQueryResult> Handle(GetReviewApprenticeshipUpdatesQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipTask = _apiClient.GetWithResponseCode<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            var updatesTask = _apiClient.GetWithResponseCode<GetApprenticeshipUpdatesResponse>(new GetApprenticeshipUpdatesRequest(request.ApprenticeshipId, Convert.ToByte(ApprenticeshipStatus.WaitingToStart)));

            await Task.WhenAll(updatesTask, apprenticeshipTask);

            var apprenticeshipResponse = apprenticeshipTask.Result;
            var apprenticeshipUpdateResponse = updatesTask.Result;

            if (apprenticeshipUpdateResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apprenticeshipUpdateResponse.EnsureSuccessStatusCode();

            var apprenticeshipUpdates = apprenticeshipUpdateResponse.Body;

            if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            apprenticeshipResponse.EnsureSuccessStatusCode();

            var apprenticeship = apprenticeshipResponse.Body;


            if (!apprenticeship.CheckParty(_serviceParameters))
            {
                return null;
            }

            if (apprenticeshipUpdates.ApprenticeshipUpdates.Count == 1)
            {
                var update = apprenticeshipUpdates.ApprenticeshipUpdates.First();
                var reviewApprenticeshipUpdatesQueryResult = new GetReviewApprenticeshipUpdatesQueryResult
                {
                    ProviderName = apprenticeship.ProviderName,
                    EmployerName = apprenticeship.EmployerName,
                    IsValidCourseCode = true,
                    ApprenticeshipUpdates = new ApprenticeshipDetails
                    {
                        FirstName = update.FirstName,
                        LastName = update.LastName,
                        Email = update.Email,
                        DateOfBirth = update.DateOfBirth,
                        Cost = update.Cost,
                        StartDate = update.StartDate,
                        EndDate = update.EndDate,
                        CourseCode = update.TrainingCode,
                        CourseName = update.TrainingName,
                        Version = update.Version,
                        Option = update.Option,
                        DeliveryModel = update.DeliveryModel,
                        EmploymentEndDate = update.EmploymentEndDate,
                        EmploymentPrice = update.EmploymentPrice
                    },
                    OriginalApprenticeship = new ApprenticeshipDetails
                    {
                        FirstName = apprenticeship.FirstName,
                        LastName = apprenticeship.LastName,
                        Email = apprenticeship.Email,
                        DateOfBirth = apprenticeship.DateOfBirth,
                        Uln = apprenticeship.Uln,
                        StartDate = apprenticeship.StartDate,
                        EndDate = apprenticeship.EndDate,
                        CourseCode = apprenticeship.CourseCode,
                        CourseName = apprenticeship.CourseName,
                        Version = apprenticeship.Version,
                        Option = apprenticeship.Option,
                        DeliveryModel = !string.IsNullOrWhiteSpace(apprenticeship.DeliveryModel) ? (DeliveryModel?)Enum.Parse(typeof(DeliveryModel), apprenticeship.DeliveryModel) : null,
                        EmploymentEndDate = apprenticeship.EmploymentEndDate,
                        EmploymentPrice = apprenticeship.EmploymentPrice
                    }
                };


                if (!string.IsNullOrWhiteSpace(update.TrainingCode))
                {
                    var providerStandardsData = await _providerStandardsService.GetStandardsData(apprenticeship.ProviderId);

                    var standard = providerStandardsData.Standards.FirstOrDefault(x => x.CourseCode.Trim() == update.TrainingCode.Trim());

                    reviewApprenticeshipUpdatesQueryResult.IsValidCourseCode = standard != null;
                }


                if (update.Cost.HasValue)
                {
                    var priceEpisodesResponse = await _apiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(request.ApprenticeshipId));
                    if (priceEpisodesResponse.StatusCode == HttpStatusCode.NotFound)
                    {
                        return null;
                    }

                    priceEpisodesResponse.EnsureSuccessStatusCode();

                    var priceEpisodes = priceEpisodesResponse.Body;
                    reviewApprenticeshipUpdatesQueryResult.OriginalApprenticeship.Cost = priceEpisodes.PriceEpisodes.GetPrice();
                }

                return reviewApprenticeshipUpdatesQueryResult;
            }
            throw new Exception("Multiple pending updates found");
        }
    }
}