using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetSkillsAndStrengths;
public class GetSkillsAndStrengthsQueryHandler : IRequestHandler<GetSkillsAndStrengthsQuery, GetSkillsAndStrengthsQueryResult>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public GetSkillsAndStrengthsQueryHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _candidateApiClient = candidateApiClient;
    }

    public async Task<GetSkillsAndStrengthsQueryResult> Handle(GetSkillsAndStrengthsQuery request, CancellationToken cancellationToken)
    {
        var application = await _candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId));
        if (application is null) throw new InvalidOperationException($"Application is null");

        var vacancy = await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(new GetVacancyRequest(application.VacancyReference));
        if (vacancy is null) throw new InvalidOperationException($"Vacancy is null");

        return new GetSkillsAndStrengthsQueryResult
        {
            ApplicationId = application.Id,
            Employer = vacancy.EmployerName,
            ExpectedSkillsAndStrengths = vacancy.Skills
        };
    }
}
