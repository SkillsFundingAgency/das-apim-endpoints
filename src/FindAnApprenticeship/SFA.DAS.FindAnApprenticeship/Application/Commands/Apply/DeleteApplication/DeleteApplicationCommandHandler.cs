using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteApplication;

public class DeleteApplicationCommandHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService
    ): IRequestHandler<DeleteApplicationCommand, DeleteApplicationCommandResult>
{
    public async Task<DeleteApplicationCommandResult> Handle(DeleteApplicationCommand request, CancellationToken cancellationToken)
    {
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        if (application is not { Status: ApplicationStatus.Draft })
        {
            return DeleteApplicationCommandResult.None;
        }

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse ??
                      await vacancyService.GetClosedVacancy(application.VacancyReference);
        
        var deleteResponse = await candidateApiClient.DeleteWithResponseCode<NullResponse>(new DeleteApplicationApiRequest(request.CandidateId, request.ApplicationId));
        return deleteResponse.StatusCode is HttpStatusCode.NoContent
            ? new DeleteApplicationCommandResult(vacancy.EmployerName, vacancy.Title)
            : DeleteApplicationCommandResult.None;
    }
}