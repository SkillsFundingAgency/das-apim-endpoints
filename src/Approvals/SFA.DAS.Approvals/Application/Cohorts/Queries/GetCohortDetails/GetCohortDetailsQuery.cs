using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.Approvals.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Queries.GetCohortDetails
{
    public class GetCohortDetailsQuery : IRequest<GetCohortDetailsQueryResult>
    {
        public long CohortId { get; set; }
    }

    public class GetCohortDetailsQueryResult
    {
        public long CohortId { get; set; }
        public string CohortReference { get; set; }
        public long AccountId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string LegalEntityName { get; set; }
        public bool HasNoDeclaredStandards { get; set; }
        public string ProviderName { get; set; }
        public long? ProviderId { get; set; }
        public bool IsFundedByTransfer { get; set; }
        public long? TransferSenderId { get; set; }
        public int? PledgeApplicationId { get; set; }
        public Party WithParty { get; set; }
        public string LatestMessageCreatedByEmployer { get; set; }
        public string LatestMessageCreatedByProvider { get; set; }
        public bool IsApprovedByEmployer { get; set; }
        public bool IsApprovedByProvider { get; set; }
        public bool IsCompleteForEmployer { get; set; }
        public bool IsCompleteForProvider { get; set; }
        public ApprenticeshipEmployerType LevyStatus { get; set; }
        public long? ChangeOfPartyRequestId { get; set; }
        public bool IsLinkedToChangeOfPartyRequest { get; set; }
        public TransferApprovalStatus? TransferApprovalStatus { get; set; }
        public LastAction LastAction { get; set; }
        public bool HasUnavailableFlexiJobAgencyDeliveryModel { get; set; }
        public IEnumerable<string> InvalidProviderCourseCodes { get; set; }
        public IEnumerable<DraftApprenticeship> DraftApprenticeships { get; set; }
        public IEnumerable<ApprenticeshipEmailOverlap> ApprenticeshipEmailOverlaps { get; set; }
        public IEnumerable<long> RplErrorDraftApprenticeshipIds { get; set; }
    }

    public class GetCohortDetailsQueryHandler : IRequestHandler<GetCohortDetailsQuery, GetCohortDetailsQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ServiceParameters _serviceParameters;
        private readonly IFjaaService _fjaaService;
        private readonly IProviderStandardsService _providerStandardsService;

        public GetCohortDetailsQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ServiceParameters serviceParameters, IFjaaService fjaaService, IProviderStandardsService providerStandardsService)
        {
            _apiClient = apiClient;
            _serviceParameters = serviceParameters;
            _fjaaService = fjaaService;
            _providerStandardsService = providerStandardsService;
        }

        public async Task<GetCohortDetailsQueryResult> Handle(GetCohortDetailsQuery request, CancellationToken cancellationToken)
        {
            var apiRequest = new GetDraftApprenticeshipsRequest(request.CohortId);
            var cohortRequest = new GetCohortRequest(request.CohortId);

            var draftApprenticeshipTask = _apiClient.GetWithResponseCode<GetDraftApprenticeshipsResponse>(apiRequest);
            var cohortResponseTask = _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);
            var emailOverlapsResponseTask = _apiClient.GetWithResponseCode<GetApprenticeshipEmailOverlapResponse>(new GetApprenticeshipEmailOverlapRequest(request.CohortId));
            var rplErrorTask = _apiClient.GetWithResponseCode<GetPriorLearningErrorResponse>(new GetPriorLearningErrorRequest(request.CohortId));

            await Task.WhenAll(draftApprenticeshipTask, cohortResponseTask, emailOverlapsResponseTask, rplErrorTask);

            if (draftApprenticeshipTask.Result.StatusCode == HttpStatusCode.NotFound || cohortResponseTask.Result.StatusCode == HttpStatusCode.NotFound ||
                emailOverlapsResponseTask.Result.StatusCode == HttpStatusCode.NotFound || rplErrorTask.Result.StatusCode == HttpStatusCode.NotFound
                )
            {
                return null;
            }

            draftApprenticeshipTask.Result.EnsureSuccessStatusCode();
            cohortResponseTask.Result.EnsureSuccessStatusCode();

            var draftApprenticeships = draftApprenticeshipTask.Result.Body;
            var emailOverlaps = emailOverlapsResponseTask.Result.Body;
            var rplErrors = rplErrorTask.Result.Body;
            var cohort = cohortResponseTask.Result.Body;

            if (!cohort.CheckParty(_serviceParameters))
            {
                return null;
            }

            var isOnRegisterTask = _fjaaService.IsAccountLegalEntityOnFjaaRegister(cohort.AccountLegalEntityId);
            var providerCoursesTask = _providerStandardsService.GetStandardsData(cohort.ProviderId);

            await Task.WhenAll(isOnRegisterTask, providerCoursesTask);

            var isOnRegister = isOnRegisterTask.Result;
            var providerCourses = providerCoursesTask.Result;

            var invalidCourses = draftApprenticeships.DraftApprenticeships
                .Select(x => x.CourseCode)
                .Where(y => !string.IsNullOrWhiteSpace(y))
                .Distinct()
                .Where(c => providerCourses.Standards.All(x => x.CourseCode != c));

            return new GetCohortDetailsQueryResult
            {
                LegalEntityName = cohort.LegalEntityName,
                ProviderName = cohort.ProviderName,
                HasNoDeclaredStandards = providerCourses.Standards?.Any() != true,
                HasUnavailableFlexiJobAgencyDeliveryModel = !isOnRegister && draftApprenticeships.DraftApprenticeships.Any(a => a.DeliveryModel.Equals(DeliveryModel.FlexiJobAgency)),
                InvalidProviderCourseCodes = invalidCourses,
                CohortId = cohort.CohortId,
                CohortReference = cohort.CohortReference,
                AccountId = cohort.AccountId,
                AccountLegalEntityId = cohort.AccountLegalEntityId,
                ProviderId = cohort.ProviderId,
                IsFundedByTransfer = cohort.IsFundedByTransfer,
                TransferSenderId = cohort.TransferSenderId,
                PledgeApplicationId = cohort.PledgeApplicationId,
                WithParty = cohort.WithParty,
                LatestMessageCreatedByEmployer = cohort.LatestMessageCreatedByEmployer,
                LatestMessageCreatedByProvider = cohort.LatestMessageCreatedByProvider,
                IsApprovedByEmployer = cohort.IsApprovedByEmployer,
                IsApprovedByProvider = cohort.IsApprovedByProvider,
                IsCompleteForEmployer = cohort.IsCompleteForEmployer,
                IsCompleteForProvider = cohort.IsCompleteForProvider,
                LevyStatus = cohort.LevyStatus,
                ChangeOfPartyRequestId = cohort.ChangeOfPartyRequestId,
                IsLinkedToChangeOfPartyRequest = cohort.IsLinkedToChangeOfPartyRequest,
                TransferApprovalStatus = cohort.TransferApprovalStatus,
                LastAction = cohort.LastAction,
                DraftApprenticeships = draftApprenticeships.DraftApprenticeships,
                ApprenticeshipEmailOverlaps = emailOverlaps.ApprenticeshipEmailOverlaps,
                RplErrorDraftApprenticeshipIds = rplErrors.DraftApprenticeshipIds
            };
        }
    }
}
