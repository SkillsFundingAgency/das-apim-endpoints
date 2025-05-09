using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Vacancies.DeleteSavedVacancy
{
    public record DeleteSavedVacancyCommandHandler(ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient) : IRequestHandler<DeleteSavedVacancyCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteSavedVacancyCommand request, CancellationToken cancellationToken)
        {
            var vacancyId = request.VacancyId.TrimVacancyReference();

            var postRequest = new PostDeleteSavedVacancyApiRequest(request.CandidateId, vacancyId, request.DeleteAllByVacancyReference);

            await CandidateApiClient.Delete(postRequest);

            return Unit.Value;
        }
    }
}
