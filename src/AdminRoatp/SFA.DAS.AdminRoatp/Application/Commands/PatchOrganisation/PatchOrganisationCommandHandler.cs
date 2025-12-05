using System.Net;
using MediatR;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.AdminRoatp.Infrastructure;
using SFA.DAS.AdminRoatp.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;

public class PatchOrganisationCommandHandler(IRoatpServiceRestApiClient _roatpServiceApiClient, IRoatpCourseManagementApiClient<RoatpV2ApiConfiguration> _roatpV2ApiClient, IRoatpServiceApiClient<RoatpConfiguration> _roatpApiClient) : IRequestHandler<PatchOrganisationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(PatchOrganisationCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _roatpServiceApiClient.PatchOrganisation(request.Ukprn, request.UserId, request.PatchDoc, cancellationToken);

        var operations = request.PatchDoc.Operations;

        var isChangeOfTypeToMain = operations.Any(x => x.path == "/ProviderType" && x.value.ToString() == ProviderType.Main.ToString());

        if (response.IsSuccessStatusCode && isChangeOfTypeToMain)
        {
            var organisationResponse = await _roatpApiClient.GetWithResponseCode<OrganisationResponse>(new GetOrganisationRequest(request.Ukprn));
            GetOrganisationQueryResult organisationDetails = organisationResponse.Body;
            var createProviderModel = new CreateProviderModel
            {
                Ukprn = request.Ukprn,
                UserId = request.UserId,
                UserDisplayName = request.UserName,
                LegalName = organisationDetails.LegalName,
                TradingName = organisationDetails.TradingName
            };

            var postProviderRequest = new PostProviderRequest(createProviderModel);
            await _roatpV2ApiClient.PostWithResponseCode<int>(postProviderRequest);
        }

        return response.StatusCode;
    }
}
