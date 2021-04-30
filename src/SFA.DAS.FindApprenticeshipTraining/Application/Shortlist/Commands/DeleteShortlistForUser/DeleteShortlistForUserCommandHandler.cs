using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistForUser
{
    public class DeleteShortlistForUserCommandHandler : IRequestHandler<DeleteShortlistForUserCommand, Unit>
    {
        private readonly ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> _courseDeliveryApiClient;

        public DeleteShortlistForUserCommandHandler (ICourseDeliveryApiClient<CourseDeliveryApiConfiguration> courseDeliveryApiClient)
        {
            _courseDeliveryApiClient = courseDeliveryApiClient;
        }
        public async Task<Unit> Handle(DeleteShortlistForUserCommand request, CancellationToken cancellationToken)
        {
            await _courseDeliveryApiClient.Delete(new DeleteShortlistForUserRequest(request.UserId));

            return new Unit();
        }
    }
}