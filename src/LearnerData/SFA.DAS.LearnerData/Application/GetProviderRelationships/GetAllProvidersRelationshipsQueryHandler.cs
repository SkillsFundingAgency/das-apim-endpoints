using System.Collections.Concurrent;
using System.Threading;
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
    private const int ProviderProcessingParallelism = 5;

    public async Task<GetAllProviderRelationshipQueryResponse?> Handle(GetAllProviderRelationshipQuery request, CancellationToken cancellationToken)
    {
        var providers = await GetRegisteredProviderDetails(request.Page, (int)request.PageSize, cancellationToken);

        if (providers is null)
        {
            return new GetAllProviderRelationshipQueryResponse() { Page = request.Page, PageSize = (int)request.PageSize, Items = [] };
        }

        ConcurrentBag<GetProviderRelationshipQueryResponse> providerResponse = [];
        using var semaphore = new SemaphoreSlim(ProviderProcessingParallelism);

        var providerTasks = providers.RegisteredProviders.Select(async provider =>
        {
            await semaphore.WaitAsync(cancellationToken);
            try
            {
                cancellationToken.ThrowIfCancellationRequested();

                var providerDetails = await getProviderRelationshipService.GetAllProviderRelationShipDetails(provider.Ukprn, cancellationToken);

                if (providerDetails is null)
                {
                    return;
                }

                var coursesForProviderTask = getProviderRelationshipService.GetCoursesForProviderByUkprn(provider.Ukprn, cancellationToken);
                var employerDetailsTask = getProviderRelationshipService.GetEmployerDetails(providerDetails, cancellationToken);

                await Task.WhenAll(coursesForProviderTask, employerDetailsTask);

                var employers = await employerDetailsTask;
                var coursesForProvider = await coursesForProviderTask;

                providerResponse.Add(new GetProviderRelationshipQueryResponse()
                {
                    Ukprn = provider.Ukprn,
                    Status = Enum.GetName(typeof(ProviderStatusType), provider.StatusId) ?? string.Empty,
                    Type = Enum.GetName(typeof(ProviderType), provider.ProviderTypeId) ?? string.Empty,
                    Employers = employers ?? [],
                    SupportedCourses = coursesForProvider?.CourseTypes ?? []
                });
            }
            finally
            {
                semaphore.Release();
            }
        });

        await Task.WhenAll(providerTasks);

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