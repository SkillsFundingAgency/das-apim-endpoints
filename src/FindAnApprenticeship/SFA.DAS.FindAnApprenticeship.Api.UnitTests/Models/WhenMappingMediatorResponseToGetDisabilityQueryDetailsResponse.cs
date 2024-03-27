using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.DisabilityConfident;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models;

public class WhenMappingMediatorResponseToGetDisabilityQueryDetailsResponse
{
    [Test, MoqAutoData]
    public void Then_Fields_Are_Mapped_Correctly(GetDisabilityConfidentDetailsQueryResult source)
    {
        var actual = (GetDisabilityConfidentDetailsApiResponse)source;

        actual.Should().BeEquivalentTo(source);
    }
}