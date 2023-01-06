using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Cohorts;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;
using Party = SFA.DAS.Approvals.InnerApi.Responses.Party;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands
{
    public class PostDetailsCommandHandler : IRequestHandler<PostDetailsCommand>
    {
        private readonly ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> _apiClient;
        private readonly ServiceParameters _serviceParameters;

        public PostDetailsCommandHandler(ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient, ServiceParameters serviceParameters)
        {
            _apiClient = apiClient;
            _serviceParameters = serviceParameters;
        }

        public async Task<Unit> Handle(PostDetailsCommand request, CancellationToken cancellationToken)
        {
            var cohortRequest = new GetCohortRequest(request.CohortId);
            var cohortResponse = await _apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);

            if (cohortResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException();
            }

            cohortResponse.EnsureSuccessStatusCode();

            var cohort = cohortResponse.Body;

            if (!CheckParty(cohort))
            {
                throw new ResourceNotFoundException();
            }

            IPostApiRequest apiRequest;

            if (request.SubmissionType == CohortSubmissionType.Approve)
            {
                apiRequest = new ApproveCohortRequest(request.CohortId);
                apiRequest.Data = new ApproveCohortRequest.Body
                {
                    RequestingParty = (Shared.Enums.Party) _serviceParameters.CallingParty,
                    Message = request.Message,
                    UserInfo = request.UserInfo
                };
            }
            else
            {
                apiRequest = new SendCohortRequest(request.CohortId);
                apiRequest.Data = new SendCohortRequest.Body
                {
                    RequestingParty = (Shared.Enums.Party)_serviceParameters.CallingParty,
                    Message = request.Message,
                    UserInfo = request.UserInfo
                };
            }
            
            var response = await _apiClient.PostWithResponseCode<EmptyResponse>(apiRequest);
            response.EnsureSuccessStatusCode();
 
            return Unit.Value;
        }

        private bool CheckParty(GetCohortResponse cohort)
        {
            switch (_serviceParameters.CallingParty)
            {
                case Party.Employer:
                {
                    if (cohort.AccountId != _serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                case Party.Provider:
                {
                    if (cohort.ProviderId != _serviceParameters.CallingPartyId)
                    {
                        return false;
                    }

                    break;
                }
                default:
                    return false;
            }

            return true;
        }
    }
}