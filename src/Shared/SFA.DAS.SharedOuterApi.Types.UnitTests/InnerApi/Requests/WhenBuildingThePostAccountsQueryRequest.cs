using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Requests.EmployerAccounts;

namespace SFA.DAS.SharedOuterApi.Types.UnitTests.InnerApi.Requests;

[TestFixture]
public class WhenBuildingThePostAccountsQueryRequest
{
    [Test]
    public void ThenPostUrlAndBodyAreCorrect()
    {
        // Arrange
        var accountIds = new long[] { 1, 2, 3 };

        // Act
        var actual = new PostAccountsQueryRequest(accountIds);

        // Assert
        actual.PostUrl.Should().Be("api/accounts/queries");
        actual.Data.Filter.AccountIds.Should().Equal(1, 2, 3);
        actual.Data.Select.Should().Contain(AccountQueryFieldNames.ApprenticeshipEmployerType);
        actual.Data.Include.Should().Contain(AccountQueryFieldNames.LegalEntities);
    }
}
