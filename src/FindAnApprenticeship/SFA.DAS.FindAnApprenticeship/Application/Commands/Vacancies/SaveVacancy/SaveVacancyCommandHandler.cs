using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.SaveVacancy
{
    public record SaveVacancyCommandHandler(ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient) : IRequestHandler<SaveVacancyCommand, SaveVacancyCommandResult>
    {
        public async Task<SaveVacancyCommandResult> Handle(SaveVacancyCommand request, CancellationToken cancellationToken)
        {
            var postData = new PostSavedVacancyApiRequestData
            {
                VacancyId = request.VacancyId,
                VacancyReference = request.VacancyId.Split('-')[0],
                CreatedOn = DateTime.UtcNow
            };

            var postRequest = new PutSavedVacancyApiRequest(request.CandidateId, postData);

            var response = await CandidateApiClient.PutWithResponseCode<PutSavedVacancyApiResponse>(postRequest);

            response.EnsureSuccessStatusCode();

            if ((int)response.StatusCode > 300)
            {
                throw new InvalidOperationException();
            }

            return response.Body;
        }
    }
}