using SFA.DAS.RecruitQa.Api.Models;
using SFA.DAS.RecruitQa.InnerApi.Responses;

namespace SFA.DAS.RecruitQa.Api.UnitTests.Models;

[TestFixture]
public class WhenMappingBlockedOrganisationRequestDto
{
    [Test, MoqAutoData]
    public void Then_The_Fields_Are_Mapped_Correctly(GetBlockedOrganisationResponse source)
    {
        //Act
        var actual = (BlockedOrganisationRequestDto)source;

        //Assert
        actual.Should().BeEquivalentTo(source);
    }
}
