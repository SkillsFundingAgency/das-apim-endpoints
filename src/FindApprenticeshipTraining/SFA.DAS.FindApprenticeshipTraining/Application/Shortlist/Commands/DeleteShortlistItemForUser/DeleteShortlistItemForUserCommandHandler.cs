using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.Configuration;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipTraining.Services;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItemForUser
{
    public class DeleteShortlistItemForUserCommandHandler : IRequestHandler<DeleteShortlistItemForUserCommand, Unit>
    {

        private readonly IShortlistApiClient<ShortlistApiConfiguration> _shortlistApiClient;

        public DeleteShortlistItemForUserCommandHandler(IShortlistApiClient<ShortlistApiConfiguration> shortlistApiClient)
        {
            _shortlistApiClient = shortlistApiClient;
        }

        public async Task<Unit> Handle(DeleteShortlistItemForUserCommand request, CancellationToken cancellationToken)
        {
            await _shortlistApiClient.Delete(new DeleteShortlistItemForUserRequest(request.Id, request.UserId));
            
            return Unit.Value;
        }
    }
}