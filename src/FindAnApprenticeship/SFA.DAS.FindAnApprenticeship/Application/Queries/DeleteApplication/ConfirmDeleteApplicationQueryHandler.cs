using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.DeleteApplication;

public class ConfirmDeleteApplicationQueryHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService
    ): IRequestHandler<ConfirmDeleteApplicationQuery, ConfirmDeleteApplicationQueryResult>
{
    public async Task<ConfirmDeleteApplicationQueryResult> Handle(ConfirmDeleteApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        if (application is not { Status: ApplicationStatus.Draft })
        {
            return ConfirmDeleteApplicationQueryResult.None;
        }

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse ??
                      await vacancyService.GetClosedVacancy(application.VacancyReference);

        return new ConfirmDeleteApplicationQueryResult
            {
                Address = vacancy.Address,
                ApplicationId = application.Id,
                ApplicationStartDate = application.CreatedDate,
                ApprenticeshipType = vacancy.ApprenticeshipType,
                EmployerLocationOption = vacancy.EmployerLocationOption,
                EmployerName = vacancy.EmployerName,
                OtherAddresses = vacancy.OtherAddresses,
                VacancyClosingDate = vacancy.ClosingDate,
                VacancyClosedDate = vacancy.ClosedDate,
                VacancyTitle = vacancy.Title,
            };
    }
}