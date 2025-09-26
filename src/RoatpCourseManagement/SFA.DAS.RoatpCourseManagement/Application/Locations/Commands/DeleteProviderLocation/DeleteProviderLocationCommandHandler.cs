using System.Threading;
using System.Threading.Tasks;
using MediatR;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Locations.Commands.DeleteProviderLocation;

public class DeleteProviderLocationCommandHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _innerApiClient) : IRequestHandler<DeleteProviderLocationCommand, ApiResponse<Unit>>
{
    public async Task<ApiResponse<Unit>> Handle(DeleteProviderLocationCommand request, CancellationToken cancellationToken)
    {
        return await _innerApiClient.DeleteWithResponseCode<Unit>((DeleteProviderLocationRequest)request);
    }
}

