using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Shortlists.Commands.DeleteExpiredShortlists;

public class DeleteExpiredShortlistsCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<DeleteExpiredShortlistsCommand>
{
    public Task Handle(DeleteExpiredShortlistsCommand request, CancellationToken cancellationToken)
    {
        return _courseManagementApiClient.Delete(new DeleteExpiredShortlistsRequest());
    }
}
