using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Courses;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ApprenticeData
{
    public class GetChangeOfEmployerApprenticeDataQueryHandler : IRequestHandler<GetChangeOfEmployerApprenticeDataQuery, GetChangeOfEmployerApprenticeDataQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;

        public GetChangeOfEmployerApprenticeDataQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
        }

        public async Task<GetChangeOfEmployerApprenticeDataQueryResult> Handle(GetChangeOfEmployerApprenticeDataQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipResponse = await _commitmentsV2ApiClient.GetWithResponseCode<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));

            if (apprenticeshipResponse.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            var priceEpisodesResponseTask = _commitmentsV2ApiClient.GetWithResponseCode<GetPriceEpisodesResponse>(new GetPriceEpisodesRequest(request.ApprenticeshipId));
            var accountLegalEntityResponseTask = _commitmentsV2ApiClient.GetWithResponseCode<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));
            var trainingProgrammeResponseTask = _commitmentsV2ApiClient.GetWithResponseCode<GetTrainingProgrammeResponse>(new GetTrainingProgrammeRequest(apprenticeshipResponse.Body.CourseCode));

            Task.WaitAll(priceEpisodesResponseTask,
               accountLegalEntityResponseTask,
               trainingProgrammeResponseTask
            );

            var priceEpisodesResponse = await priceEpisodesResponseTask;
            var accountLegalEntityResponse = await accountLegalEntityResponseTask;
            var trainingProgrammeResponse = await trainingProgrammeResponseTask;

            return new GetChangeOfEmployerApprenticeDataQueryResult
            {
                Apprenticeship = apprenticeshipResponse.Body,
                PriceEpisodes = priceEpisodesResponse.Body,
                AccountLegalEntity = accountLegalEntityResponse.Body,
                TrainingProgrammeResponse = trainingProgrammeResponse.Body
            };
        }
    }
}
