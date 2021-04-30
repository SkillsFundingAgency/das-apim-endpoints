using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItemForUser
{
    public class DeleteShortlistItemForUserCommandHandler : IRequestHandler<DeleteShortlistItemForUserCommand, Unit>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public DeleteShortlistItemForUserCommandHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<Unit> Handle(DeleteShortlistItemForUserCommand request, CancellationToken cancellationToken)
        {
            await _courseDeliveryApiClient.Delete(new DeleteShortlistItemForUserRequest(request.Id, request.UserId));
            
            return Unit.Value;
        }
    }
}