using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Commands.CreateProvider;
using SFA.DAS.AdminRoatp.Application.Commands.PostOrganisation;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Commands.PostOrganisation;
public class CreateProviderModelTests
{
    [Test, MoqAutoData]
    public void BuildsModel_FromCommand(
        PostOrganisationCommand command)
    {
        CreateProviderModel sut = command;

        sut.Should().BeEquivalentTo(command, options => options.ExcludingMissingMembers());
        sut.UserId.Should().BeEquivalentTo(command.RequestingUserId);
        sut.UserDisplayName.Should().BeEquivalentTo(command.RequestingUserDisplayName);
    }
}
