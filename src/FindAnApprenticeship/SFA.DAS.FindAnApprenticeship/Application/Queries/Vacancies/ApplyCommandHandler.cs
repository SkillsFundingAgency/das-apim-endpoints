using System;
using System.Collections.Generic;
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

public class ApplyCommandHandler(
    IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration> findApprenticeshipApiClient,
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
    : IRequestHandler<ApplyCommand, ApplyCommandResponse>
{
    public async Task<ApplyCommandResponse> Handle(ApplyCommand request, CancellationToken cancellationToken)
    {
        var result =
            await findApprenticeshipApiClient.Get<GetApprenticeshipVacancyItemResponse>(
                new GetVacancyRequest(request.VacancyReference));

        var additionalQuestions = new List<string>();
        if (result.AdditionalQuestion1 != null) { additionalQuestions.Add(result.AdditionalQuestion1); }
        if (result.AdditionalQuestion2 != null) { additionalQuestions.Add(result.AdditionalQuestion2); }

        PutApplicationApiRequest.PutApplicationApiRequestData putApplicationApiRequestData = new PutApplicationApiRequest.PutApplicationApiRequestData
        {
            CandidateId = request.CandidateId,
            AdditionalQuestions = additionalQuestions,
            IsAdditionalQuestion1Complete = string.IsNullOrEmpty(result.AdditionalQuestion1) ? (short)4 : (short)0,
            IsAdditionalQuestion2Complete = string.IsNullOrEmpty(result.AdditionalQuestion2) ? (short)4 : (short)0
        };
        var putData = putApplicationApiRequestData;
        var vacancyReference =
            request.VacancyReference.Replace("VAC", "", StringComparison.CurrentCultureIgnoreCase);
        var putRequest = new PutApplicationApiRequest(vacancyReference, putData);

        var applicationResult =
            await candidateApiClient.PutWithResponseCode<PutApplicationApiResponse>(putRequest);

        applicationResult.EnsureSuccessStatusCode();

        if (applicationResult is null) return null;

        return new ApplyCommandResponse
        {
            ApplicationId = applicationResult.Body.Id
        };

    }
}