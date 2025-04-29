using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser
{
    public class DeleteShortlistForUserCommandHandler : IRequestHandler<DeleteShortlistForUserCommand, Unit>
    {
        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;
        public DeleteShortlistForUserCommandHandler (IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _shortlistApiClient = shortlistApiClient;
        }
        public async Task<Unit> Handle(DeleteShortlistForUserCommand request, CancellationToken cancellationToken)
        {
            await _shortlistApiClient.Delete(new DeleteShortlistForUserRequest(request.UserId));

            return new Unit();
        }
    }
}