﻿using MediatR;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;
using static SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests.PostDeleteJobRequest;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteJob
{
    public class PostDeleteJobCommandHandler : IRequestHandler<PostDeleteJobCommand, Unit>
    {
        private readonly ICandidateApiClient<CandidateApiConfiguration> _apiClient;

        public PostDeleteJobCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient)
        {
            _apiClient = candidateApiClient;
        }

        public async Task<Unit> Handle(PostDeleteJobCommand command, CancellationToken cancellationToken)
        {
            var request = new PostDeleteJobRequest(command.ApplicationId, command.CandidateId, new PostDeleteJobRequestData
            {
                JobId = command.JobId,
            });

            await _apiClient.PostWithResponseCode<NullResponse>(request);
            return Unit.Value;
        }
    }

}
