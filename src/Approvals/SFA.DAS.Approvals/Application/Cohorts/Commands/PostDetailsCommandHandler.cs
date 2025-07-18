﻿using System.Net;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.Approvals.Application.Shared.Enums;
using SFA.DAS.Approvals.Exceptions;
using SFA.DAS.Approvals.Extensions;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Requests.Cohorts;
using SFA.DAS.Approvals.InnerApi.CommitmentsV2Api.Responses;
using SFA.DAS.Approvals.InnerApi.Requests;
using SFA.DAS.Approvals.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Approvals.Application.Cohorts.Commands
{
    public class PostDetailsCommandHandler(
        ICommitmentsV2ApiClient<CommitmentsV2ApiConfiguration> apiClient,
        ServiceParameters serviceParameters)
        : IRequestHandler<PostDetailsCommand, Unit>
    {
        public async Task<Unit> Handle(PostDetailsCommand request, CancellationToken cancellationToken)
        {
            var cohortRequest = new GetCohortRequest(request.CohortId);
            var cohortResponse = await apiClient.GetWithResponseCode<GetCohortResponse>(cohortRequest);

            if (cohortResponse.StatusCode == HttpStatusCode.NotFound)
            {
                throw new ResourceNotFoundException();
            }

            cohortResponse.EnsureSuccessStatusCode();

            var cohort = cohortResponse.Body;

            if (!cohort.CheckParty(serviceParameters))
            {
                throw new ResourceNotFoundException();
            }

            IPostApiRequest apiRequest;

            if (request.SubmissionType == CohortSubmissionType.Approve)
            {
                apiRequest = new ApproveCohortRequest(request.CohortId);
                apiRequest.Data = new ApproveCohortRequest.Body
                {
                    RequestingParty = serviceParameters.CallingParty,
                    Message = request.Message,
                    UserInfo = request.UserInfo
                };
            }
            else
            {
                apiRequest = new SendCohortRequest(request.CohortId);
                apiRequest.Data = new SendCohortRequest.Body
                {
                    RequestingParty = serviceParameters.CallingParty,
                    Message = request.Message,
                    UserInfo = request.UserInfo
                };
            }
            
            var response = await apiClient.PostWithResponseCode<EmptyResponse>(apiRequest, false);
            response.EnsureSuccessStatusCode();
 
            return Unit.Value;
        }
    }
}