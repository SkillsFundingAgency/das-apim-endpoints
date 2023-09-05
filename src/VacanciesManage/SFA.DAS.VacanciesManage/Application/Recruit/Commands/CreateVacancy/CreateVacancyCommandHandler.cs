using MediatR;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.VacanciesManage.Configuration;
using SFA.DAS.VacanciesManage.Enums;
using SFA.DAS.VacanciesManage.InnerApi.Requests;
using SFA.DAS.VacanciesManage.InnerApi.Responses;
using SFA.DAS.VacanciesManage.Interfaces;
using System;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.VacanciesManage.Application.Recruit.Commands.CreateVacancy
{
    public class CreateVacancyCommandHandler : IRequestHandler<CreateVacancyCommand, CreateVacancyCommandResponse>
    {
        private readonly IRecruitApiClient<RecruitApiConfiguration> _recruitApiClient;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;
        private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient;

        public CreateVacancyCommandHandler (IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, IAccountLegalEntityPermissionService accountLegalEntityPermissionService, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> roatpCourseManagementApiClient)
        {
            _recruitApiClient = recruitApiClient;
            _accountLegalEntityPermissionService = accountLegalEntityPermissionService;
            _roatpCourseManagementApiClient = roatpCourseManagementApiClient;
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
                if (request.AccountIdentifier.Ukprn.HasValue && !await IsTrainingProviderMainOrEmployerProfile(Convert.ToInt32(request.AccountIdentifier.Ukprn.Value)))
                {
                    throw new HttpRequestContentException(
                        $"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})",
                        HttpStatusCode.BadRequest,
                        $"Enter a UKPRN of a training provider who is registered to deliver apprenticeship training: UkPrn:{ request.AccountIdentifier.Ukprn }");
                }
                request.PostVacancyRequestData.EmployerAccountId = accountLegalEntity.AccountHashedId;
            }

            if (request.PostVacancyRequestData.EmployerNameOption == EmployerNameOption.RegisteredName)
            {
                request.PostVacancyRequestData.EmployerName = accountLegalEntity.Name;
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

        /// <summary>
        /// Method to check if the given ukprn number is a valid training provider with Main or Employer Profile with Status not equal to "Not Currently Starting New Apprentices".
        /// </summary>
        /// <param name="ukprn">ukprn number.</param>
        /// <returns>boolean.</returns>
        private async Task<bool> IsTrainingProviderMainOrEmployerProfile(int ukprn)
        {
            var provider = await _roatpCourseManagementApiClient.Get<GetProvidersListItem>(new GetProviderRequest(ukprn));

            // logic to filter only Training provider with Main & Employer Profiles and Status Id not equal to "Not Currently Starting New Apprentices"
            return provider != null &&
                   (provider.ProviderTypeId.Equals((short)ProviderTypeIdentifier.MainProvider)
                    || provider.ProviderTypeId.Equals((short)ProviderTypeIdentifier.EmployerProvider))
                   && !provider.StatusId.Equals((short)ProviderStatusType.ActiveButNotTakingOnApprentices);
        }
    }
}