using AutoFixture;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.LearnerData.Application.GetProviderRelationships;
using SFA.DAS.LearnerData.Enums;
using SFA.DAS.LearnerData.Responses;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.RoatpV2;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LearnerData.UnitTests.Application.GetProviderRelationships;

[TestFixture]
public class WhenGettingProviderRelationships
{
    private GetProviderRelationshipQueryHandler _sut;
    private Mock<IGetProviderRelationshipService> _getProviderRelationshipService;
    private Mock<IRoatpV2TrainingProviderService> _roatpService;

    [SetUp]
    public void SetUp()
    {
        _getProviderRelationshipService = new Mock<IGetProviderRelationshipService>();
        _roatpService = new Mock<IRoatpV2TrainingProviderService>();
        _sut = new GetProviderRelationshipQueryHandler(_getProviderRelationshipService.Object, _roatpService.Object);
    }

    [Test]
    public async Task CanCallHandle()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();
        var providerLegalEntitiesresponse = fixture.Create<GetProviderAccountLegalEntitiesResponse>();
        var providerSummary = fixture.Create<GetProviderSummaryResponse>();

        var employers = fixture.Create<List<EmployerDetails>>();

        _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.IsAny<int>())).
            ReturnsAsync(providerLegalEntitiesresponse);

        _roatpService.Setup(t => t.GetProviderSummary(It.IsAny<int>())).
            ReturnsAsync(providerSummary);

        _getProviderRelationshipService.Setup(t => t.GetEmployerDetails(It.IsAny<GetProviderAccountLegalEntitiesResponse>())).
            ReturnsAsync(employers);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().NotBeNull();
        result?.UkPRN.Should().BeEquivalentTo(request.Ukprn.ToString());
        result?.Status.Should().Be(Enum.GetName(typeof(ProviderStatusType),providerSummary.StatusId));
        result?.Type.Should().Be(Enum.GetName(typeof(ProviderType), providerSummary.ProviderTypeId));
        result?.Employers?.Length.Should().Be(providerLegalEntitiesresponse.AccountProviderLegalEntities.Count);
    }

    [Test]
    public async Task WhenProviderRelationshipsIsnull_ReturnsNull()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task WhenProviderIsnull_ReturnsNull()
    {
        // Arrange
        var fixture = new Fixture();
        var request = fixture.Create<GetProviderRelationshipQuery>();
        var cancellationToken = fixture.Create<CancellationToken>();
        var providerLegalEntitiesresponse = fixture.Create<GetProviderAccountLegalEntitiesResponse>();

        _getProviderRelationshipService.Setup(t => t.GetAllProviderRelationShipDetails(It.IsAny<int>())).
          ReturnsAsync(providerLegalEntitiesresponse);

        // Act
        var result = await _sut.Handle(request, cancellationToken);

        // Assert
        result.Should().BeEquivalentTo(new GetProviderRelationshipQueryResponse());
    }
}