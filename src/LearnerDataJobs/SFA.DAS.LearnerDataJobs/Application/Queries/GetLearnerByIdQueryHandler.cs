using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.LearnerDataJobs.Application.Commands;
using SFA.DAS.LearnerDataJobs.InnerApi;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFA.DAS.LearnerDataJobs.Application.Queries
{
    public class GetLearnerByIdQueryHandler(IInternalApiClient<LearnerDataInnerApiConfiguration> client, ILogger<GetLearnerByIdQueryHandler> logger)
    : IRequestHandler<GetLearnerByIdQuery, GetLearnerByIdResult>
    {
        public async Task<GetLearnerByIdResult> Handle(GetLearnerByIdQuery command, CancellationToken cancellationToken)
        {
            logger.LogTrace("Handling GetLearnerByIdQuery");
            var request = new GetLearnerByIdRequest(command.ukprn, command.Id);

            var learner = await client.Get<GetLearnerDataByIdResponse>(request);

            if (learner == null)
            {
                logger.LogTrace($"Learner Data  does not exists for Id : {command.Id}");
                return new GetLearnerByIdResult();
            }

            return new GetLearnerByIdResult
            {
                ApprenticeshipId = learner.ApprenticeshipId,
            };
        }
    }
}
