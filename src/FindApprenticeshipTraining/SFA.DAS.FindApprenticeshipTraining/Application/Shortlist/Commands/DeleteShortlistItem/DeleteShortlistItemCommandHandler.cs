using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;

public class DeleteShortlistItemCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<DeleteShortlistItemCommand, Unit>
{
    public async Task<Unit> Handle(DeleteShortlistItemCommand request, CancellationToken cancellationToken)
    {
        await _roatpCourseManagementApiClient.Delete(new DeleteShortlistItemRequest(request.ShortlistId));

        return Unit.Value;
    }
}