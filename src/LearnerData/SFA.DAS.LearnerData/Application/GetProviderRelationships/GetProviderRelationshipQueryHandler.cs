using MediatR;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Types.Interfaces;

namespace SFA.DAS.LearnerData.Application.GetProviderRelationships;

public class GetProviderRelationshipQueryHandler(
    IGetProviderRelationshipService getProviderRelationshipService,
    IRoatpV2TrainingProviderService roatpService)
    : IRequestHandler<GetProviderRelationshipQuery, GetProviderRelationshipQueryResponse?>
{
    public async Task<GetProviderRelationshipQueryResponse?> Handle(GetProviderRelationshipQuery request, CancellationToken cancellationToken)
    {
        var providerDetailsTask = getProviderRelationshipService.GetAllProviderRelationShipDetails(request.Ukprn, cancellationToken);
        var providerSummaryTask = GetRegisteredProviderDetails(request.Ukprn, cancellationToken);

        await Task.WhenAll(providerDetailsTask, providerSummaryTask);

        var providerDetails = await providerDetailsTask;
        if (providerDetails is null)
        {
            return null;
        }

        var provider = await providerSummaryTask;
        if (provider is null)
        {
            return null;
        }

        var coursesForProviderTask = getProviderRelationshipService.GetCoursesForProviderByUkprn(request.Ukprn, cancellationToken);
        var employerDetailsTask = getProviderRelationshipService.GetEmployerDetails(providerDetails, cancellationToken);

        await Task.WhenAll(coursesForProviderTask, employerDetailsTask);

        var employers = await employerDetailsTask;
        var coursesForProvider = await coursesForProviderTask;

        return new GetProviderRelationshipQueryResponse
        {
            Ukprn = request.Ukprn,
            Status = Enum.GetName(typeof(ProviderStatusType), provider.StatusId) ?? string.Empty,
            Type = Enum.GetName(typeof(ProviderType), provider.ProviderTypeId) ?? string.Empty,
            Employers = employers ?? [],
            SupportedCourses = coursesForProvider?.CourseTypes ?? []
        };
    }

    private async Task<GetProviderSummaryResponse?> GetRegisteredProviderDetails(int ukprn, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return await roatpService.GetProviderSummary(ukprn);
    }
}
