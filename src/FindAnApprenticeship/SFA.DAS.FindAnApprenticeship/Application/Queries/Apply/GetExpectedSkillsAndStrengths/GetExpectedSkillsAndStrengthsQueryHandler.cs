using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetExpectedSkillsAndStrengths;
public class GetExpectedSkillsAndStrengthsQueryHandler : IRequestHandler<GetExpectedSkillsAndStrengthsQuery, GetExpectedSkillsAndStrengthsQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetExpectedSkillsAndStrengthsQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetExpectedSkillsAndStrengthsQueryResult> Handle(GetExpectedSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        var application = await _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        if (application is null) throw new InvalidOperationException($"Application is null");

        var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(application.VacancyReference));
        if (vacancy is null) throw new InvalidOperationException($"Vacancy is null");

        bool? isCompleted = application.SkillsAndStrengthStatus switch
        {
            Constants.SectionStatus.Incomplete => false,
            Constants.SectionStatus.Completed => true,
            _ => null
        };

        return new GetExpectedSkillsAndStrengthsQueryResult
        {
            ApplicationId = application.Id,
            Employer = vacancy.EmployerName,
            ExpectedSkillsAndStrengths = vacancy.Skills,
            IsSectionCompleted = isCompleted
        };
    }
}
