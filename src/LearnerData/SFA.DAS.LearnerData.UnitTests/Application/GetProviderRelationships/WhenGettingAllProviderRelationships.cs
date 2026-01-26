using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.GetProviderRelationships;

[TestFixture]
public class WhenGettingAllProviderRelationships
{
    [Test, MoqAutoData]
    public async Task GetEmployerDetails_Calls_With_Correct_Values(
        GetAllProviderRelationshipQuery request,
        CancellationToken cancellation,
        GetProviderAccountLegalEntitiesResponse[] providerLegalEntitiesresponse,
        GetProvidersResponse providerSummary,
        List<List<EmployerDetails>> employers,
        Mock<IGetProviderRelationshipService> _getProviderRelationshipService,
        Mock<IRoatpV2TrainingProviderService> _roatpService
        )
    {
        // Arrange

        int count = providerSummary.RegisteredProviders.Count();
        _roatpService.Setup(t => t.GetProviders(cancellation)).
         ReturnsAsync(providerSummary);

        int index = 0;
        foreach (var provider in providerSummary.RegisteredProviders)
        {
            var providerDetails = providerLegalEntitiesresponse[index];
            var employerDetails = employers[index];

            _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.Is<int>(ukprn => ukprn == provider.Ukprn))).
           ReturnsAsync(providerDetails);

            _getProviderRelationshipService.Setup(t => t.GetEmployerDetails(It.Is<GetProviderAccountLegalEntitiesResponse>(x => x == providerDetails))).
                ReturnsAsync(employerDetails);
            index++;
        }

        GetAllProvidersRelationshipsQueryHandler _sut = new GetAllProvidersRelationshipsQueryHandler(_roatpService.Object, _getProviderRelationshipService.Object);

        // Act
        var result = await _sut.Handle(request, cancellation);

        // Assert
        result.Should().NotBeNull();
        result.Items.Should().HaveCount(providerSummary.RegisteredProviders.Count());
        result.Page.Should().Be(request.Page);
        result.PageSize.Should().Be(request.PageSize);
        result.TotalItems.Should().Be(providerSummary.TotalCount);

        index = 0;

        foreach (var provider in providerSummary.RegisteredProviders)
        {
            var response = result.Items.First(r => r.UkPRN == provider.Ukprn.ToString());
            response.Status.Should().Be(Enum.GetName(typeof(ProviderStatusType), provider.StatusId));
            response.Type.Should().Be(Enum.GetName(typeof(ProviderType), provider.ProviderTypeId));
            response.Employers.Should().BeEquivalentTo(employers[index]);
        }
    }

    [Test, MoqAutoData]
    public async Task WhenProviderIsnull_ReturnsEmptyEmployerDetails(
        GetAllProviderRelationshipQuery request,
        CancellationToken cancellation,
        GetProvidersResponse response,
        Mock<IGetProviderRelationshipService> _getProviderRelationshipService,
        Mock<IRoatpV2TrainingProviderService> _roatpService
        )
    {
        // Arrange
        _roatpService.Setup(t => t.GetProviders(cancellation)).
            ReturnsAsync((GetProvidersResponse?)null);

        GetAllProvidersRelationshipsQueryHandler _sut = new GetAllProvidersRelationshipsQueryHandler(_roatpService.Object, _getProviderRelationshipService.Object);

        // Act
        var result = await _sut.Handle(request, cancellation);

        // Assert
        result?.Items.Should().HaveCount(0);
        result?.Page.Should().Be(request.Page);
        result?.PageSize.Should().Be(request.PageSize);
    }

    [Test, MoqAutoData]
    public async Task WhenProviderRelationshipsIsnull_NoEmployers(
        int ukprnValue,
        GetAllProviderRelationshipQuery request,
        GetProvidersResponse response,
        GetProviderAccountLegalEntitiesResponse providerLegalEntitiesresponse,
        Mock<IGetProviderRelationshipService> _getProviderRelationshipService,
        Mock<IRoatpV2TrainingProviderService> _roatpService
        )
    {
        // Arrange
        _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.IsAny<int>()));

        _roatpService.Setup(t => t.GetProviders(CancellationToken.None)).
          ReturnsAsync(response);

        GetAllProvidersRelationshipsQueryHandler _sut = new GetAllProvidersRelationshipsQueryHandler(_roatpService.Object, _getProviderRelationshipService.Object);

        // Act
        var result = await _sut.Handle(request, CancellationToken.None);

        // Assert
        result?.Items.Should().HaveCount(0);
    }
}