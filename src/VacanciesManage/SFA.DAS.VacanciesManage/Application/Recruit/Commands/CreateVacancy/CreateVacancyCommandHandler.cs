using System;
using System.Linq;
using System.Net;
using System.Security;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.SharedOuterApi.Common;
using SFA.DAS.SharedOuterApi.Configuration;
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
        private readonly IRecruitApiClient<RecruitApiV2Configuration> _recruitApiV2Client;
        private readonly IAccountLegalEntityPermissionService _accountLegalEntityPermissionService;
        private readonly ICourseService _courseService;

        public CreateVacancyCommandHandler (IRecruitApiClient<RecruitApiConfiguration> recruitApiClient, IRecruitApiClient<RecruitApiV2Configuration> recruitApiV2Client, IAccountLegalEntityPermissionService accountLegalEntityPermissionService, ICourseService courseService)
        {
            _recruitApiClient = recruitApiClient;
            _recruitApiV2Client = recruitApiV2Client;
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
            request.PostVacancyV2RequestData.LegalEntityName = accountLegalEntity.Name;
            
            if(request.AccountIdentifier.AccountType == AccountType.Provider)
            {
                request.PostVacancyRequestData.EmployerAccountId = accountLegalEntity.AccountHashedId;
                request.PostVacancyV2RequestData.OwnerType = nameof(OwnerType.Provider);
            }
            else
            {
                request.PostVacancyV2RequestData.OwnerType = nameof(OwnerType.Employer);
            }
            
            request.PostVacancyV2RequestData.AccountId = accountLegalEntity.AccountId;
            request.PostVacancyV2RequestData.AccountLegalEntityId = accountLegalEntity.AccountLegalEntityId;

            if (request.PostVacancyRequestData.EmployerNameOption == EmployerNameOption.RegisteredName)
            {
                request.PostVacancyRequestData.EmployerName = accountLegalEntity.Name;
                request.PostVacancyV2RequestData.EmployerName = accountLegalEntity.Name;
            }

            var standards = await _courseService.GetActiveStandards<GetStandardsListResponse>(nameof(GetStandardsListResponse));

            var course = standards.Standards.FirstOrDefault(c =>
                c.LarsCode.ToString() == request.PostVacancyRequestData.ProgrammeId);
            request.PostVacancyRequestData.ApprenticeshipType = "Standard";
            
            if (course is { ApprenticeshipType: ApprenticeshipType.FoundationApprenticeship })
            {
                request.PostVacancyRequestData.Qualifications = [];
                request.PostVacancyRequestData.Skills = [];
                request.PostVacancyRequestData.ApprenticeshipType = "Foundation";
                request.PostVacancyV2RequestData.Qualifications = [];
                request.PostVacancyV2RequestData.Skills = [];
                request.PostVacancyV2RequestData.ApprenticeshipType = "Foundation";
            }
            
            string vacancyReference;
            if (request.IsSandbox)
            {
                if (course != null && ((course.LastDateStarts != null && course.LastDateStarts < request.PostVacancyV2RequestData.StartDate) ||
                                                  (course.EffectiveTo !=null && course.EffectiveTo < request.PostVacancyV2RequestData.StartDate)))
                {
                    var dateToDisplay = course.LastDateStarts ?? course.EffectiveTo.Value;
                    
                    var message = $"Start date must be on or before {dateToDisplay} as this is the last day for new starters for the training course you have selected. If you don't want to change the start date, you can change the training course";
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)HttpStatusCode.BadRequest} ({HttpStatusCode.BadRequest})", HttpStatusCode.BadRequest, message);
                }
                var postValidateVacancyRequest = new PostVacancyV2Request(request.PostVacancyV2RequestData);

                var result = await _recruitApiV2Client.PostWithResponseCode<PostVacancyResponse>(postValidateVacancyRequest);
                
                HandleHttpResponseError(result);
                
                vacancyReference = result.Body.VacancyReference.ToString();
            }
            else
            {
                var apiRequest = new PostVacancyRequest(request.Id, request.PostVacancyRequestData.User.Ukprn, request.PostVacancyRequestData.User.Email, request.PostVacancyRequestData);
                var result = await _recruitApiClient.PostWithResponseCode<long?>(apiRequest);

                HandleHttpResponseError(result);

                vacancyReference = result.Body.ToString();
            }
            
            return new CreateVacancyCommandResponse
            {
                VacancyReference = vacancyReference
            };
        }

        private static void HandleHttpResponseError<T>(ApiResponse<T> result)
        {
            if(!((int)result.StatusCode >= 200 && (int)result.StatusCode <= 299))
            {
                if (result.StatusCode.Equals(HttpStatusCode.BadRequest))
                {
                    throw new HttpRequestContentException($"Response status code does not indicate success: {(int)result.StatusCode} ({result.StatusCode})", result.StatusCode, result.ErrorContent);    
                }

                throw new Exception(
                    $"Response status code does not indicate success: {(int) result.StatusCode} ({result.StatusCode})");
            }
        }
    }
}