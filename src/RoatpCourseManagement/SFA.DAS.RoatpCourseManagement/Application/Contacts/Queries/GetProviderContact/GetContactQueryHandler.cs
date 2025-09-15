using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.RoatpCourseManagement.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.RoatpCourseManagement.Application.Contacts.Queries.GetProviderContact;

public class GetContactQueryHandler : IRequestHandler<GetContactQuery, ApiResponse<GetContactResponse>>
{
    private readonly IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient;
    private readonly ILogger<GetContactQueryHandler> _logger;

    public GetContactQueryHandler(IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> courseManagementApiClient, ILogger<GetContactQueryHandler> logger)
    {
        _courseManagementApiClient = courseManagementApiClient;
        _logger = logger;
    }

    public async Task<ApiResponse<GetContactResponse>> Handle(GetContactQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Get Provider contact request received for ukprn {Ukprn}", request.Ukprn);

        return await _courseManagementApiClient.GetWithResponseCode<GetContactResponse>(new GetContactRequest(request.Ukprn));
    }
}