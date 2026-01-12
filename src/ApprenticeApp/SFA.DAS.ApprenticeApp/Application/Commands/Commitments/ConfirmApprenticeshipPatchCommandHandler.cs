using MediatR;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeApp.Application.Commands.Commitments
{
    public class ConfirmApprenticeshipPatchCommandHandler 
        : IRequestHandler<ConfirmApprenticeshipPatchCommand, Unit>
    {
        private readonly 
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> _client;
        
        public ConfirmApprenticeshipPatchCommandHandler(
            IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration> client)
        {
            _client = client;
        }

        public async Task<Unit> Handle(ConfirmApprenticeshipPatchCommand command,
            CancellationToken cancellationToken)
        {
            var confirmApprenticeshipPatchRequest = new ConfirmApprenticeshipPatchRequest(command.ApprenticeId,
                command.ApprenticeshipId, command.RevisionId)
            {
                Data = command.Patch
            };

            await _client.Patch(confirmApprenticeshipPatchRequest);

            return Unit.Value;
        }
    }
}
