using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.LearnerData.Services;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LearnerData.UnitTests.Application.Services;

[TestFixture]
public class GetProviderRelationshipServiceTests
{
    [Test, MoqAutoData]
    public async Task CanCallGetEmployerDetails(
        GetProviderAccountLegalEntitiesResponse providerDetails,
        GetProviderRelationshipService _sut)
    {
        // Act
        var details = await _sut.GetEmployerDetails(providerDetails);

        details.Should().NotBeNull();
        details.Should().HaveCount(providerDetails.AccountProviderLegalEntities.Count);
        details.Select(t => t.AgreementId).Should().BeEquivalentTo(providerDetails.AccountProviderLegalEntities.Select(t => t.AccountLegalEntityPublicHashedId));

        var firstItem = details.First();
        var secondItem = providerDetails.AccountProviderLegalEntities.Select(t => t.AccountLegalEntityPublicHashedId == firstItem.AgreementId);
    }
}