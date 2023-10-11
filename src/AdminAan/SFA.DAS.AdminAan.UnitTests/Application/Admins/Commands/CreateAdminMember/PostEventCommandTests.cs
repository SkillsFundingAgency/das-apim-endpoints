using FluentAssertions;
using SFA.DAS.AdminAan.Application.CalendarEvents.Commands.Create;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.AdminAan.UnitTests.Application.Admins.Commands.CreateAdminMember;
public class PostEventCommandTests
{
    [Test, MoqAutoData]
    public void Operator_Mapping(CreateEventRequestModel source)
    {
        PostEventCommand command = source;
        command.Should().BeEquivalentTo(source);
    }
}
