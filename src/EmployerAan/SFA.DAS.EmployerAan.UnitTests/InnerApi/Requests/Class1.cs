using FluentAssertions;
using SFA.DAS.EmployerAan.InnerApi.Profiles;

namespace SFA.DAS.EmployerAan.UnitTests.InnerApi.Requests;

public class GetProfilesByUserTypeRequestTests
{
    [Test]
    public void Initialise_PopulatesCorrectUrl()
    {
        GetProfilesByUserTypeRequest sut = new(Guid.NewGuid().ToString());

        sut.GetUrl.Should().Be($"/profiles/{sut.UserType}");
    }
}
