using MediatR;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.DeleteQualifications;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Users.DeleteCandidate
{
    public record DeleteCandidateCommandHandler(ICandidateApiClient<CandidateApiConfiguration> CandidateApiClient) : IRequestHandler<DeleteQualificationsCommand>
    {
        public async Task Handle(DeleteCandidateCommand command, CancellationToken cancellationToken)
        {
            if (request.Id.HasValue)
            {
                var apiRequest = new DeleteAccountApiRequest(command.CandidateId);
                var response = await CandidateApiClient.Delete<DeleteAccountApiResponse>(apiRequest);
            }
            else
            {
                var apiRequest = new DeleteQualificationsByTypeApiRequest(request.ApplicationId, request.CandidateId, request.QualificationReferenceId);
                await candidateApiClient.Delete(apiRequest);
            }
        }
    }
}
