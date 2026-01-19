using System.Collections.Concurrent;
using System.Linq;
using MediatR;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
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

        if (providers is null)
        {
            return new GetAllProviderRelationshipQueryResponse() { Page = request.Page, PageSize = request.PageSize };
        }

        ConcurrentBag<GetProviderRelationshipQueryResponse> providerResponse = [];

        await Parallel.ForEachAsync(providers.RegisteredProviders,
                new ParallelOptions { MaxDegreeOfParallelism = 5 },
                async (p, cancellationToken) =>
                {
                    var providerDetails = await getProviderRelationshipService.GetAllProviderRelationShipDetails(p.Ukprn);

                    if (providerDetails is null)
                    {
                        return;
                    }

                    var employerDetails = await getProviderRelationshipService.GetEmployerDetails(providerDetails);

                    providerResponse.Add(new GetProviderRelationshipQueryResponse()
                    {
                        UkPRN = p.Ukprn.ToString(),
                        Status = Enum.GetName(typeof(ProviderStatusType), p.StatusId)??string.Empty,
                        Type = Enum.GetName(typeof(ProviderType), p.ProviderTypeId) ?? string.Empty,
                        Employers = employerDetails.ToArray()
                    });
                });

        return new GetAllProviderRelationshipQueryResponse() { GetAllProviderRelationships = [.. providerResponse], Page = request.Page, PageSize = request.PageSize };
    }

    private async Task<GetProvidersResponse?> GetRegisteredProviderDetails(int page, int? pageSize, CancellationToken cancellationToken)
    {
        
        var providerDetails = await roatpService.GetProviders(cancellationToken);
        if (providerDetails is null) { return null; }
        providerDetails.RegisteredProviders = [.. providerDetails.RegisteredProviders.Skip((page - 1) * (int)pageSize).Take((int)pageSize)];
        return providerDetails;
    }
}