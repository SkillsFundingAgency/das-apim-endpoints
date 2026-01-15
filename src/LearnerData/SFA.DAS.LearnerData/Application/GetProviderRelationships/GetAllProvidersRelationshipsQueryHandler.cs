using MediatR;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetAllProvidersRelationshipsQueryHandler(
    IRoatpV2TrainingProviderService roatpService,
    IGetProviderRelationshipService getProviderRelationshipService)
    : IRequestHandler<GetAllProviderRelationshipQuery, GetAllProviderRelationshipQueryResponse?>
{
    public async Task<GetAllProviderRelationshipQueryResponse?> Handle(GetAllProviderRelationshipQuery request, CancellationToken cancellationToken)
    {
        var providers = await GetRegisteredProviderDetails(request.Page, request.PageSize, cancellationToken);

        if (providers is null) return null;

        List<GetProviderRelationshipQueryResponse> providerResponse = [];

        await Parallel.ForEachAsync(providers.RegisteredProviders,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (p, cancellationToken) =>
                {
                    var providerDetails = await getProviderRelationshipService.GetAllProviderRelationShipDetails(p.Ukprn);

                    if (providerDetails is null)
                    {
                        return;
                    }

               var employerDetails =   await getProviderRelationshipService.GetEmployerDetails(providerDetails);

                    providerResponse.Add(new GetProviderRelationshipQueryResponse()
                    {
                        UkPRN = p.Ukprn.ToString(),
                        Status = p.StatusId,
                        Type = p.ProviderTypeId,
                        Employers = [.. employerDetails]
                    });
                });

        return new GetAllProviderRelationshipQueryResponse() { GetAllProviderRelationships = providerResponse };
    }   

    private async Task<GetProvidersResponse> GetRegisteredProviderDetails(int page, int pagesize, CancellationToken cancellationToken)
    {
        var providerDetails = await roatpService.GetProviders(cancellationToken);
        providerDetails.RegisteredProviders = [.. providerDetails.RegisteredProviders.Skip(page - 1 * pagesize).Take(pagesize)];
        return providerDetails;
    }
}