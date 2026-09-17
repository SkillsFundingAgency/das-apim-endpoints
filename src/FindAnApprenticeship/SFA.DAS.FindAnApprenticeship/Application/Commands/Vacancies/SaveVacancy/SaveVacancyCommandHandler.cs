using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Services;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy
{
    public class SaveVacancyCommandHandler(
        ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
        IMetrics metrics) : IRequestHandler<SaveVacancyCommand, SaveVacancyCommandResult>
    {
        public async Task<SaveVacancyCommandResult> Handle(SaveVacancyCommand request, CancellationToken cancellationToken)
        {
            // Avoid string array allocation from Split
            var vacancyId = request.VacancyId;
            var separatorIndex = vacancyId.IndexOf('-');
            var vacancyReference = separatorIndex >= 0
                ? vacancyId[..separatorIndex]
                : vacancyId;

            var createdOn = DateTime.UtcNow;

            var postData = new PostSavedVacancyApiRequestData
            {
                VacancyId = vacancyId,
                VacancyReference = vacancyReference,
                CreatedOn = createdOn
            };

            var postRequest = new PutSavedVacancyApiRequest(request.CandidateId, postData);

            var response = await candidateApiClient.PutWithResponseCode<PutSavedVacancyApiResponse>(postRequest);

            response.EnsureSuccessStatusCode();

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            metrics.IncreaseVacancySaved(vacancyReference);

            return response.Body;
        }
    }
}