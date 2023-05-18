using FluentAssertions;
using SFA.DAS.EmployerAan.InnerApi.Requests;

namespace SFA.DAS.EmployerAan.UnitTests.InnerApi.Requests;
public class GetEmployerMemberRequestTests
{
    [Test]
    public void Initialise_PopulatesCorrectUrl()
    {
        GetEmployerMemberRequest sut = new() { UserRef = Guid.NewGuid() };

        sut.GetUrl.Should().Be($"/employers/{sut.UserRef}");
    }
}
