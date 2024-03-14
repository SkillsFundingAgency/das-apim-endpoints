using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident
{
    public class GetDisabilityConfidentQuery : IRequest<GetDisabilityConfidentQueryResult>
    {
        public Guid CandidateId { get; set; }
        public Guid ApplicationId { get; set; }
    }

    public class GetDisabilityConfidentQueryResult
    {
        public string EmployerName { get; set; }
        public bool? ApplyUnderDisabilityConfidentScheme { get; set; }
        public bool? IsSectionCompleted { get; set; }
    }

    public class GetDisabilityConfidentQueryHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient) : IRequestHandler<GetDisabilityConfidentQuery, GetDisabilityConfidentQueryResult>
    {
        public async Task<GetDisabilityConfidentQueryResult> Handle(GetDisabilityConfidentQuery request, CancellationToken cancellationToken)
        {
            var applicationRequest = new GetApplicationApiRequest(request.CandidateId, request.ApplicationId);
            var application = await candidateApiClient.Get<GetApplicationApiResponse>(applicationRequest);

            if (application == null)
            {
                return null;
            }

            var vacancyRequest = new GetVacancyRequest(application.VacancyReference);
            var vacancy = await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(vacancyRequest);

            bool? isCompleted = application.DisabilityConfidenceStatus switch
            {
                Domain.Constants.SectionStatus.Incomplete => false,
                Domain.Constants.SectionStatus.Completed => true,
                _ => null
            };

            return new GetDisabilityConfidentQueryResult
            {
                EmployerName = vacancy.EmployerName,
                ApplyUnderDisabilityConfidentScheme = application.ApplyUnderDisabilityConfidentScheme,
                IsSectionCompleted = isCompleted
            };
        }
    }
}
