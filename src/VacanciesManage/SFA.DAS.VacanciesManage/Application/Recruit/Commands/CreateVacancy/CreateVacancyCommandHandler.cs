using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;
        private readonly ICourseService _courseService;

        public CreateVacancyCommandHandler (IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, IAccountLegalEntityPermissionService accountLegalEntityPermissionService, ICourseService courseService)
        {
            _recruitApiClient = recruitApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
            _courseService = courseService;
        }
        
        public async Task<CreateVacancyCommandResponse> Handle(CreateVacancyCommand request, CancellationToken cancellationToken)
        {
            var accountLegalEntity = await _accountLegalEntityPermissionService.GetAccountLegalEntity(
                request.AccountIdentifier, request.PostVacancyRequestData.AccountLegalEntityPublicHashedId);

            if (accountLegalEntity == null)
            {
                throw new SecurityException();
            }
            
            request.PostVacancyRequestData.LegalEntityName = accountLegalEntity.Name;
            
            if(request.AccountIdentifier.AccountType == AccountType.Provider)
            {
                request.PostVacancyRequestData.EmployerAccountId = accountLegalEntity.AccountHashedId;
            }

            if (request.PostVacancyRequestData.EmployerNameOption == EmployerNameOption.RegisteredName)
            {
                request.PostVacancyRequestData.EmployerName = accountLegalEntity.Name;
            }

            var standardsTask = await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            var standard = standardsTask.Standards.FirstOrDefault(c =>
                c.LarsCode.ToString() == request.PostVacancyRequestData.ProgrammeId);

            if (standard == null)
            {
                throw new InvalidOperationException($"Standard with ProgrammeId '{request.PostVacancyRequestData.ProgrammeId}' not found.");
            }

            var standardApprenticeshipType = standard.ApprenticeshipType.Contains("Foundation", StringComparison.CurrentCultureIgnoreCase) ? 
                ApprenticeshipTypes.Foundation : ApprenticeshipTypes.Standard;

            if(request.PostVacancyRequestData.ApprenticeshipType != standardApprenticeshipType)
            {
                throw new ArgumentException($"Apprenticeship Type does not match the definition for ProgrammeId '{request.PostVacancyRequestData.ProgrammeId}'");
            }

            IPostApiRequest apiRequest;
            if (request.IsSandbox)
            {
                apiRequest = new PostValidateVacancyRequest(request.Id, request.PostVacancyRequestData);
            }
            else
            {
                apiRequest = new PostVacancyRequest(request.Id, request.PostVacancyRequestData.User.Ukprn, request.PostVacancyRequestData.User.Email, request.PostVacancyRequestData);

            }

            var result = await _recruitApiClient.PostWithResponseCode<long?>(apiRequest);

            if(!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);    
                }

                throw new Exception(
                    $"Response status code does not indicate success: {(int) result.StatusCode} ({result.StatusCode})");
            }
            
            return new CreateVacancyCommandResponse
            {
                VacancyReference = result.Body.ToString()
            };
        }
    }
}