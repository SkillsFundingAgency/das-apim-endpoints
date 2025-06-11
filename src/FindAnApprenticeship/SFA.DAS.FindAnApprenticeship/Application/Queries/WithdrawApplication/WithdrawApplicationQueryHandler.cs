using MediatR;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Queries.WithdrawApplication;

public class WithdrawApplicationQueryHandler(
    ICandidateApiClient<CandidateApiConfiguration> candidateApiClient,
    IVacancyService vacancyService) : IRequestHandler<WithdrawApplicationQuery, WithdrawApplicationQueryResult>
{
    public async Task<WithdrawApplicationQueryResult> Handle(WithdrawApplicationQuery request, CancellationToken cancellationToken)
    {
        var application = await candidateApiClient.Get<GetApplicationApiResponse>(new GetApplicationApiRequest(request.CandidateId, request.ApplicationId, false));
        if (application is not { Status: ApplicationStatus.Submitted })
        {
            return new WithdrawApplicationQueryResult();
        }

        var vacancy = await vacancyService.GetVacancy(application.VacancyReference) as GetApprenticeshipVacancyItemResponse ??
                      await vacancyService.GetClosedVacancy(application.VacancyReference);

        return new WithdrawApplicationQueryResult
        {
            ApplicationId = application.Id,
            ClosingDate = vacancy.ClosingDate,
            ClosedDate = vacancy.ClosedDate,
            EmployerName = vacancy.EmployerName,
            SubmittedDate = application.SubmittedDate,
            AdvertTitle = vacancy.Title,
            Address = vacancy.Address,
            OtherAddresses = vacancy.OtherAddresses,
            EmployerLocationOption = vacancy.EmployerLocationOption,
            EmploymentLocationInformation = vacancy.EmploymentLocationInformation,
            ApprenticeshipType = vacancy.ApprenticeshipType
        };
    }
}