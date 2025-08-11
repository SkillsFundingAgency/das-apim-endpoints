using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using SFA.DAS.RoatpCourseManagement.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.RoatpCourseManagement.Application.Contacts.Commands;

public class CreateProviderContactCommandHandler(ILogger<CreateProviderContactCommandHandler> _logger, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _courseManagementApiClient) : IRequestHandler<CreateProviderContactCommand, int>
{
    public async Task<int> Handle(CreateProviderContactCommand request, CancellationToken cancellationToken)
    {
        var apiRequest = new CreateProviderContactRequest(request);
        _logger.LogInformation("Request to create provider contact for ukprn: {Ukprn} by user: {UserId}", request.Ukprn, request.UserId);
        var response = await _courseManagementApiClient.PostWithResponseCode<int>(apiRequest);
        return (int)response.StatusCode;
    }
}