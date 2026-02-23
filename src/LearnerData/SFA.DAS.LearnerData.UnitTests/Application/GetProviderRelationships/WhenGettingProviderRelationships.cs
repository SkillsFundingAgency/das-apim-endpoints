using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.GetProviderRelationships;

[TestFixture]
public class WhenGettingProviderRelationships
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Calls_With_Correct_Values(
        GetProviderAccountLegalEntitiesResponse providerLegalEntitiesresponse,
        GetProviderSummaryResponse providerSummary,
        GetProviderRelationshipQuery request,
        List<EmployerDetails> employers,
       [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
       [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
       [Greedy] GetProviderRelationshipQueryHandler  sut)
    {
        // Arrange
        providerSummary.StatusId = 1;
        providerSummary.ProviderTypeId = 1;

        getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(request.Ukprn)).
            ReturnsAsync(providerLegalEntitiesresponse);

        roatpService.Setup(t => t.GetProviderSummary(request.Ukprn)).
            ReturnsAsync(providerSummary);

        getProviderRelationshipService.Setup(t => t.GetEmployerDetails(providerLegalEntitiesresponse)).
            ReturnsAsync(employers);
        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Ukprn.Should().BeEquivalentTo(request.Ukprn.ToString());
        result.Status.Should().Be(Enum.GetName(typeof(ProviderStatusType), providerSummary.StatusId));
        result.Type.Should().Be(Enum.GetName(typeof(ProviderType), providerSummary.ProviderTypeId));
        result.Employers?.Length.Should().Be(providerLegalEntitiesresponse.AccountProviderLegalEntities.Count);
    }

    [Test, MoqAutoData]
    public async Task WhenProviderRelationshipsIsnull_ReturnsNull(
        GetProviderRelationshipQuery request,
       [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
       [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
       [Greedy] GetProviderRelationshipQueryHandler sut)
    {
        // Arrange
        getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(request.Ukprn)).
            ReturnsAsync((GetProviderAccountLegalEntitiesResponse?)null);

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Test, MoqAutoData]
    public async Task WhenProviderIsnull_ReturnsNull(
        int ukprnValue,
    GetProviderAccountLegalEntitiesResponse providerLegalEntitiesresponse,
    GetProviderRelationshipQuery request,
   [Frozen] Mock<IGetProviderRelationshipService> getProviderRelationshipService,
   [Frozen] Mock<IRoatpV2TrainingProviderService> roatpService,
   [Greedy] GetProviderRelationshipQueryHandler sut)
    {
        // Arrange
        getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(request.Ukprn)).
          ReturnsAsync(providerLegalEntitiesresponse);

        roatpService.Setup(t => t.GetProviderSummary(request.Ukprn))
            .ReturnsAsync((GetProviderSummaryResponse?)null);

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }
}