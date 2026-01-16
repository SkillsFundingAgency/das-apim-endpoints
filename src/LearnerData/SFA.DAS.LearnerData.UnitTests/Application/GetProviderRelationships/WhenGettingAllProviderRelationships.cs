using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.GetProviderRelationships;

[TestFixture]
public class WhenGettingAllProviderRelationships
{
    private GetAllProvidersRelationshipsQueryHandler _sut;
    private Mock<IGetProviderRelationshipService> _getProviderRelationshipService;
    private Mock<IRoatpV2TrainingProviderService> _roatpService;

    [SetUp]
    public void SetUp()
    {
        _getProviderRelationshipService = new Mock<IGetProviderRelationshipService>();
        _roatpService = new Mock<IRoatpV2TrainingProviderService>();
        _sut = new GetAllProvidersRelationshipsQueryHandler(_roatpService.Object, _getProviderRelationshipService.Object);
    }

    [Test]
    public async Task CanCallHandle()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetAllProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();
        var providerLegalEntitiesresponse = fixture.Create<GetProviderAccountLegalEntitiesResponse>();
        var providerSummary = fixture.Create<GetProvidersResponse>();

        var employers = fixture.Create<List<EmployerDetails>>();

        _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.IsAny<int>())).
            ReturnsAsync(providerLegalEntitiesresponse);

        _roatpService.Setup(t => t.GetProviders(cancellationToken)).
            ReturnsAsync(providerSummary);

        _getProviderRelationshipService.Setup(t => t.GetEmployerDetails(It.IsAny<GetProviderAccountLegalEntitiesResponse>())).
            ReturnsAsync(employers);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result.GetAllProviderRelationships.Should().HaveCount(providerSummary.RegisteredProviders.Count());
        result.GetAllProviderRelationships.Select(x => x.UkPRN).Should().Equal(providerSummary.RegisteredProviders.Select(t => t.Ukprn.ToString()));
    }

    [Test]
    public async Task WhenProviderIsnull_ReturnsNull()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetAllProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();

        _roatpService.Setup(t => t.GetProviders(cancellationToken)).
           ReturnsAsync((GetProvidersResponse?)null);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenProviderRelationshipsIsnull_NoEmployers()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetAllProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();
        var providerLegalEntitiesresponse = fixture.Create<GetProviderAccountLegalEntitiesResponse>();

        _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.IsAny<int>())).
          ReturnsAsync((GetProviderAccountLegalEntitiesResponse?)null);

        _roatpService.Setup(t => t.GetProviders(cancellationToken)).
          ReturnsAsync(fixture.Create<GetProvidersResponse>());

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result?.GetAllProviderRelationships.Should().HaveCount(0);
    }
}