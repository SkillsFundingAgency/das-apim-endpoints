using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Vacancies.Manage.Configuration;
using SFA.DAS.Vacancies.Manage.InnerApi.Requests;
using SFA.DAS.Vacancies.Manage.InnerApi.Responses;
using SFA.DAS.Vacancies.Manage.Interfaces;

namespace SFA.DAS.Vacancies.Manage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateTraineeshipVacancyCommandHandler : IRequestHandler<CreateTraineeshipVacancyCommand, CreateTraineeshipVacancyCommandResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;

        public CreateTraineeshipVacancyCommandHandler(IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, IAccountLegalEntityPermissionService accountLegalEntityPermissionService)
        {
            _recruitApiClient = recruitApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
        }

        public async Task<CreateTraineeshipVacancyCommandResponse> Handle(CreateTraineeshipVacancyCommand request, CancellationToken cancellationToken)
        {
            var accountLegalEntity = await _accountLegalEntityPermissionService.GetAccountLegalEntity(
                request.AccountIdentifier, request.PostVacancyRequestData.AccountLegalEntityPublicHashedId);

            if (accountLegalEntity == null)
            {
                throw new SecurityException();
            }

            request.PostVacancyRequestData.LegalEntityName = accountLegalEntity.Name;

            request.PostVacancyRequestData.EmployerAccountId = accountLegalEntity.AccountHashedId;

            if (request.PostVacancyRequestData.EmployerNameOption == TraineeshipEmployerNameOption.RegisteredName)
            {
                request.PostVacancyRequestData.EmployerName = accountLegalEntity.Name;
            }

            IPostApiRequest apiRequest;
            if (request.IsSandbox)
            {
                apiRequest = new PostValidateTraineeshipVacancyRequest(request.Id, request.PostVacancyRequestData);
            }
            else
            {
                apiRequest = new PostTraineeshipVacancyRequest(request.Id, request.PostVacancyRequestData.User.Ukprn, "", request.PostVacancyRequestData);
            }

            var result = await _recruitApiClient.PostWithResponseCode<string>(apiRequest);

            if (!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);
                }

                throw new Exception(
                    $"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})");
            }

            return new CreateTraineeshipVacancyCommandResponse
            {
                VacancyReference = result.Body
            };
        }
    }
}