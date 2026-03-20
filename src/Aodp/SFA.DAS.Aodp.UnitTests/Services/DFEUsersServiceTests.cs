using Moq;
using NUnit.Framework;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.ExternalApi.DfeSignIn;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models.DfeSignIn;

namespace SFA.DAS.Aodp.Services;

[TestFixture]
public class DfeUsersServiceTests
{
    private Mock<IDfeUsersApiClient<DfeSignInApiConfiguration>> _clientMock;
    private DfeUsersService _sut;

    [SetUp]
    public void SetUp()
    {
        _clientMock = new Mock<IDfeUsersApiClient<DfeSignInApiConfiguration>>();
        _sut = new DfeUsersService(_clientMock.Object);
    }

    [Test]
    public async Task Then_GetUsersByRoleAsync_Calls_Client_And_Returns_Users()
    {
        // arrange
        var ukprn = "12345678";
        var role = "Reviewer";

        var expectedUsers = new List<User>
        {
            new User { Email = "a@test.com", FirstName = "A", LastName="Test" },
            new User { Email = "b@test.com", FirstName="B", LastName="Test" }
        };

        var apiResponse = new OrganisationUsersResponse
        {
            Users = expectedUsers
        };

        _clientMock
            .Setup(x => x.Get<OrganisationUsersResponse>(It.IsAny<GetUsersByRoleApiRequest>()))
            .ReturnsAsync(apiResponse);

        // act
        var result = await _sut.GetUsersByRoleAsync(ukprn, role);

        // assert
        Assert.That(result, Is.Not.Null);
        Assert.That(result, Is.EqualTo(expectedUsers));

        _clientMock.Verify(x =>
            x.Get<OrganisationUsersResponse>(It.Is<GetUsersByRoleApiRequest>(r =>
                r != null)), 
            Times.Once);
    }

    [Test]
    public void Then_When_Client_Throws_Exception_Is_Propagated()
    {
        // arrange
        _clientMock
            .Setup(x => x.Get<OrganisationUsersResponse>(It.IsAny<GetUsersByRoleApiRequest>()))
            .ThrowsAsync(new Exception("boom"));

        // act + assert
        Assert.ThrowsAsync<Exception>(() => _sut.GetUsersByRoleAsync("12345678", "Reviewer"));
    }
}
