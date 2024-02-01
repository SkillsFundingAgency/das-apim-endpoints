using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.Vacancies;

public class ApplyCommandHandler : IRequestHandler<ApplyCommand, ApplyCommandResponse>
{
    private readonly IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> _findApprenticeshipApiClient;
    private readonly ICandidateApiClient<CandidateApiConfiguration> _candidateApiClient;

    public ApplyCommandHandler(
        IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    {
        _findApprenticeshipApiClient = findApprenticeshipApiClient;
        _candidateApiClient = candidateApiClient;
    }

    public async Task<ApplyCommandResponse> Handle(ApplyCommand request, CancellationToken cancellationToken)
    {
        var result =
            await _findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(
                new GetVacancyRequest(request.VacancyReference));

        var putData = new PutApplicationApiRequest.PutApplicationApiRequestData
        {
            CandidateId = request.CandidateId
        };
        var vacancyReference =
            request.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
        var putRequest = new PutApplicationApiRequest(vacancyReference, putData);

        var applicationResult =
            await _candidateApiClient.PutWithResponseCode<PutApplicationApiResponse>(putRequest);

        applicationResult.EnsureSuccessStatusCode();

        if (result is null) return null;
        if (applicationResult is null) return null;

        return new ApplyCommandResponse
        {
            ApplicationId = applicationResult.Body.Id
        };

    }
}