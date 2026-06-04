using System.Collections.Concurrent;
using MediatR;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.Apim.Shared.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetAllProvidersRelationshipsQueryHandler(
    IRoatpV2TrainingProviderService roatpService,
    IGetProviderRelationshipService getProviderRelationshipService)
    : IRequestHandler<GetAllProviderRelationshipQuery, GetAllProviderRelationshipQueryResponse?>
{
    public async Task<GetAllProviderRelationshipQueryResponse?> Handle(GetAllProviderRelationshipQuery request, CancellationToken cancellationToken)
    {
        var providers = await GetRegisteredProviderDetails(request.Page, (int)request.PageSize, cancellationToken);

        if (providers is null)
        {
            return new GetAllProviderRelationshipQueryResponse() { Page = request.Page, PageSize = (int)request.PageSize, Items = [] };
        }

        ConcurrentBag<GetProviderRelationshipQueryResponse> providerResponse = [];

        await Parallel.ForEachAsync(providers.RegisteredProviders,
                new ParallelOptions { MaxDegreeOfParallelism = 5, CancellationToken = cancellationToken },
                async (p, token) =>
                {
                    var providerDetails = await getProviderRelationshipService.GetAllProviderRelationShipDetails(p.Ukprn, token);

                    if (providerDetails is null)
                    {
                        return;
                    }

                    var coursesForProviderTask = getProviderRelationshipService.GetCoursesForProviderByUkprn(p.Ukprn, token);
                    var employerDetailsTask = getProviderRelationshipService.GetEmployerDetails(providerDetails, token);

                    await Task.WhenAll(coursesForProviderTask, employerDetailsTask);

                    var employers = await employerDetailsTask;
                    var coursesForProvider = await coursesForProviderTask;

                    providerResponse.Add(new GetProviderRelationshipQueryResponse()
                    {
                        Ukprn = p.Ukprn,
                        Status = Enum.GetName(typeof(ProviderStatusType), p.StatusId) ?? string.Empty,
                        Type = Enum.GetName(typeof(ProviderType), p.ProviderTypeId) ?? string.Empty,
                        Employers = employers ?? [],
                        SupportedCourses = coursesForProvider?.CourseTypes ?? []
                    });
                });

        return new GetAllProviderRelationshipQueryResponse() { Page = request.Page, PageSize = (int)request.PageSize, TotalItems = providers.TotalCount, Items = providerResponse.ToList() };
    }

    private async Task<GetProvidersResponse?> GetRegisteredProviderDetails(int page, int pageSize, CancellationToken cancellationToken)
    {
        var providerDetails = await roatpService.GetProviders(cancellationToken);
        if (providerDetails is null) { return null; }
        providerDetails.TotalCount = providerDetails.RegisteredProviders.Count();
        providerDetails.RegisteredProviders = [.. providerDetails.RegisteredProviders.Skip((page - 1) * pageSize).Take(pageSize)];
        return providerDetails;
    }
}