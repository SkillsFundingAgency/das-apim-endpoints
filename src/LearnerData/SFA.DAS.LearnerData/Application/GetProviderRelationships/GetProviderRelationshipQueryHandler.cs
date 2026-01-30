using MediatR;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetProviderRelationshipQueryHandler(
    IGetProviderRelationshipService getProviderRelationshipService,
    IRoatpV2TrainingProviderService roatpService)
    : IRequestHandler<GetProviderRelationshipQuery, GetProviderRelationshipQueryResponse?>
{
    public async Task<GetProviderRelationshipQueryResponse?> Handle(GetProviderRelationshipQuery request, CancellationToken cancellationToken)
    {
        var providerDetails = await getProviderRelationshipService.GetAllProviderRelationShipDetails(request.Ukprn);

        if (providerDetails is null)
        {
            return null;
        }
        var provider = await GetRegisteredProviderDetails(request.Ukprn);

        if (provider is null)
        {
            return null;
        }

        var employerDetails = await getProviderRelationshipService.GetEmployerDetails(providerDetails);

        return new GetProviderRelationshipQueryResponse()
        {
            UkPRN = request.Ukprn.ToString(),
            Status = Enum.GetName(typeof(ProviderStatusType), provider.StatusId) ?? string.Empty,
            Type = Enum.GetName(typeof(ProviderType), provider.ProviderTypeId) ?? string.Empty,
            Employers = employerDetails.ToArray()
        };
    }

    private async Task<GetProviderSummaryResponse> GetRegisteredProviderDetails(int ukprn)
    {
        var providerDetails = await roatpService.GetProviderSummary(ukprn);
        return providerDetails;
    }
}