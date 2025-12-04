using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.SharedOuterApi.InnerApi.Requests.Roatp;

namespace SFA.DAS.SharedOuterApi.UnitTests.Models;
public class PostOrganisationRequestTests
{
    [Test, MoqAutoData]
    public void BuildsModel_FromCommand(
        PostOrganisationCommand command)
    {
        PostOrganisationRequest sut = command;

        sut.Should().BeEquivalentTo(command, options => options
            .Excluding(x => x.RequestingUserId)
            .Excluding(x => x.RequestingUserDisplayName)
            .Excluding(x => x.DeliversApprenticeships)
            .Excluding(x => x.DeliversApprenticeshipUnits)
        );

        sut.RequestingUserId.Should().BeEquivalentTo(command.RequestingUserDisplayName);
    }
}
