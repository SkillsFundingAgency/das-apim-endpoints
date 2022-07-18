using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Apprentices.Queries.ChangeEmployer.ConfirmEmployer
{
    public class GetConfirmEmployerQueryHandler : IRequestHandler<GetConfirmEmployerQuery, GetConfirmEmployerQueryResult>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _commitmentsV2ApiClient;
        private readonly IFjaaApiClient<FjaaApiConfiguration> _fjaaClient;
        private readonly FeatureToggles _featureToggles;

        public GetConfirmEmployerQueryHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> commitmentsV2ApiClient, IFjaaApiClient<FjaaApiConfiguration> fjaaClient, FeatureToggles featureToggles)
        {
            _commitmentsV2ApiClient = commitmentsV2ApiClient;
            _fjaaClient = fjaaClient;
            _featureToggles = featureToggles;
        }

        public async Task<GetConfirmEmployerQueryResult> Handle(GetConfirmEmployerQuery request, CancellationToken cancellationToken)
        {
            var apprenticeshipTask = _commitmentsV2ApiClient.Get<GetApprenticeshipResponse>(new GetApprenticeshipRequest(request.ApprenticeshipId));
            var accountLegalEntityTask = _commitmentsV2ApiClient.Get<GetAccountLegalEntityResponse>(new GetAccountLegalEntityRequest(request.AccountLegalEntityId));

            await Task.WhenAll(apprenticeshipTask, accountLegalEntityTask);

            var apprenticeship = apprenticeshipTask.Result;
            var accountLegalEntity = accountLegalEntityTask.Result;

            var isFlexiJobAgency = await IsLegalEntityOnFjaaRegister(accountLegalEntity.MaLegalEntityId);

            if (apprenticeship == null || apprenticeship.ProviderId != request.ProviderId)
            {
                return null;
            }

            return new GetConfirmEmployerQueryResult
            {
                AccountLegalEntityName = accountLegalEntity.LegalEntityName,
                AccountName = accountLegalEntity.AccountName,
                LegalEntityName = apprenticeship.EmployerName,
                IsFlexiJobAgency = isFlexiJobAgency,
                DeliveryModel = apprenticeship.DeliveryModel
            };
        }

        private async Task<bool> IsLegalEntityOnFjaaRegister(long legalEntityId)
        {
            if (!_featureToggles.ApprovalsFeatureToggleFjaaEnabled)
            {
                return false;
            }

            var agencyRequest = await _fjaaClient.GetWithResponseCode<GetAgencyResponse>(new GetAgencyRequest(legalEntityId));

            if (agencyRequest.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            agencyRequest.EnsureSuccessStatusCode();
            return true;
        }
    }
}