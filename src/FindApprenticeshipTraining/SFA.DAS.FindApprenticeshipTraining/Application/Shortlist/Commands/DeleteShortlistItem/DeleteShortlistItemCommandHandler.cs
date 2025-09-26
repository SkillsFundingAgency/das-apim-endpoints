using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.FindApprenticeshipTraining.Application.Shortlist.Commands.DeleteShortlistItem;

public class DeleteShortlistItemCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpCourseManagementApiClient) : IRequestHandler<DeleteShortlistItemCommand, DeleteShortlistItemCommandResult>
{
    public async Task<DeleteShortlistItemCommandResult> Handle(DeleteShortlistItemCommand request, CancellationToken cancellationToken)
    {
        var response = await _roatpCourseManagementApiClient.DeleteWithResponseCode<DeleteShortlistItemCommandResult>(new DeleteShortlistItemRequest(request.ShortlistId), true);

        response.EnsureSuccessStatusCode();

        return response.Body;
    }
}