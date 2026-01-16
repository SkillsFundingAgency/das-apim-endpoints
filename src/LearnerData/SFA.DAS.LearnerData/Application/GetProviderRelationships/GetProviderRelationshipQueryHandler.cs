using MediatR;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
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
        var provider = await GetRegisteredProviderDetails(request.Ukprn, cancellationToken);
        if (provider is null) { return null; }

        var employerDetails = await getProviderRelationshipService.GetEmployerDetails(providerDetails);

        return new GetProviderRelationshipQueryResponse()
        {
            UkPRN = request.Ukprn.ToString(),
            Status = provider.StatusId,
            Type = provider.ProviderTypeId,
            Employers = [.. employerDetails]
        };
    }

    private async Task<GetProviderSummaryResponse> GetRegisteredProviderDetails(int ukprn, CancellationToken cancellationToken)
    {
        var providerDetails = await roatpService.GetProviderSummary(ukprn);
        return providerDetails;
    }
}