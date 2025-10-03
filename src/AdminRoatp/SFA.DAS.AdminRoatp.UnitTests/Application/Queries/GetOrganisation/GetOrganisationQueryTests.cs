using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetOrganisation;
public class GetOrganisationQueryTests
{
    [Test, MoqAutoData]

    public void GetOrganisationRequest_ReturnsCorrectGetUrl(
        GetOrganisationQuery request)
    {
        var expectedUrl = $"organisations/{request.ukprn}";

        GetOrganisationRequest apiRequest = new(request.ukprn);

        apiRequest.GetUrl.Should().Be(expectedUrl);
    }
}
