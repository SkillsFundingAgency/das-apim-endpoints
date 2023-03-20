using System.Web;
using AutoFixture.NUnit3;
using FluentAssertions;
using SFA.DAS.RoatpOversight.Application.Commands.CreateProvider;
using SFA.DAS.RoatpOversight.InnerApi.Requests;

namespace SFA.DAS.RoatpOversight.UnitTests.InnerApi.Requests;

public class CreateProviderRequestTests
{
    [Test]
    [AutoData]
    public void Ctor_PopulatesRequest(CreateProviderCommand command)
    {
        var expectedUrl = $"providers?userId={HttpUtility.UrlEncode(command.UserId)}&userDisplayName={HttpUtility.UrlEncode(command.UserDisplayName)}";

        CreateProviderRequest sut = new(command);

        sut.Data.Should().Be(command);
        sut.PostUrl.Should().Be(expectedUrl);
    }
}
