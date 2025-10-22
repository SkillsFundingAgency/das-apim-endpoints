using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;

public class GetOrganisationQueryResultTests
{
    [Test, AutoData]
    public void GetOrganisationQueryResult_ImplicitOperator_MapsPropertiesCorrectly(OrganisationResponse source)
    {
        GetOrganisationQueryResult result = source;
        // Assert
        source.Should().BeEquivalentTo(result, options => options.ExcludingMissingMembers());
    }
}
