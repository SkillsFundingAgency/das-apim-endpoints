using MediatR;
using SFA.DAS.Apim.Shared.Extensions;
using SFA.DAS.Assessor.Contracts.ApiRequests;
using SFA.DAS.Assessor.Contracts.ApiResponses;
using SFA.DAS.Assessor.Contracts.Client;
using SFA.DAS.Assessor.Models;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.Assessor.Application.Queries.GetUser
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, GetUserResult>
    {
        private readonly IAssessorApiClient<AssessorApiConfiguration> _assessorApiClient;

        public GetUserQueryHandler(
            IAssessorApiClient<AssessorApiConfiguration> assessorApiClient)
        {
            _assessorApiClient = assessorApiClient;
        }

        public async Task<GetUserResult> Handle(
            GetUserQuery query,
            CancellationToken cancellationToken)
        {
            var response = await _assessorApiClient
                .GetWithResponseCode<GetUserResponse>(
                    new GetUsersByDfeSignInUserIdApiRequest(
                        query.DfeSignInUserId));

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return new GetUserResult();
            }

            response.EnsureSuccessStatusCode();

            if (response.Body == null)
            {
                throw new InvalidOperationException(
                    "The Assessor API returned a successful response without a user response body.");
            }

            return new GetUserResult
            {
                User = (User)response.Body
            };
        }
    }
}