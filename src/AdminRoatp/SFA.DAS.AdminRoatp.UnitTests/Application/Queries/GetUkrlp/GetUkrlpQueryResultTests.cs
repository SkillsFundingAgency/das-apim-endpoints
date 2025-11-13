using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetUkrlp;
public class GetUkrlpQueryResultTests
{
    [Test]
    public void ImplicitConversion_AllDataPresent_MapsAllDataCorrectly()
    {
        // Arrange
        var providerDetails = new ProviderDetails
        {
            ProviderName = "TestName1",
            ProviderAliases = new List<ProviderAlias>
            {
                new ProviderAlias
                {
                    Alias = "TestAlias1"
                },
            },
            VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = "charity commission",
                    VerificationId = "12345"
                },
                new VerificationDetails
                {
                    VerificationAuthority = "companies house",
                    VerificationId = "67890"
                }
            }
        };

        // Act
        GetUkrlpQueryResult sut = providerDetails;

        // Assert
        sut.LegalName.Should().Be("TestName1");
        sut.TradingName.Should().Be("TestAlias1");
        sut.CharityNumber.Should().Be("12345");
        sut.CompanyNumber.Should().Be("67890");
    }

    [TestCase("Charity Commission", "12345", "12345", null)]
    [TestCase("Companies House", "67890", null, "67890")]
    [TestCase("Test Authority", "54321", null, null)]
    [TestCase(null, null, null, null)]
    public void ImplicitConversion_VariationOfVerificationDetails_MapsCharityAndCompanyNumberCorrectly(string? verificationAuthority, string? verificationId, string? expectedCharityNumber, string? expectedCompanyNumber)
    {
        // Arrange
        var providerDetails = new ProviderDetails
        {
            ProviderName = "TestName1",
            ProviderAliases = new List<ProviderAlias>
            {
                new ProviderAlias
                {
                    Alias = "TestAlias1"
                },
            },
            VerificationDetails = new List<VerificationDetails>
            {
                new VerificationDetails
                {
                    VerificationAuthority = verificationAuthority,
                    VerificationId = verificationId
                }
            }
        };

        // Act
        GetUkrlpQueryResult sut = providerDetails;

        // Assert
        sut.LegalName.Should().Be("TestName1");
        sut.TradingName.Should().Be("TestAlias1");
        sut.CharityNumber.Should().Be(expectedCharityNumber);
        sut.CompanyNumber.Should().Be(expectedCompanyNumber);
    }

    [Test]
    public void ImplicitConversion_DataIsNull_MapsNullValues()
    {
        // Arrange
        var providerDetails = new ProviderDetails
        {
            ProviderName = null,
            ProviderAliases = null,
            VerificationDetails = null
        };

        // Act
        GetUkrlpQueryResult sut = providerDetails;

        // Assert
        sut.LegalName.Should().BeNull();
        sut.TradingName.Should().BeNull();
        sut.CharityNumber.Should().BeNull();
        sut.CompanyNumber.Should().BeNull();
    }

    [Test]
    public void ImplicitConversion_DataIsEmpty_MapsNullValues()
    {
        // Arrange
        var providerDetails = new ProviderDetails
        {
            ProviderName = "TestName1",
            ProviderAliases = new List<ProviderAlias>(),
            VerificationDetails = new List<VerificationDetails>()
        };

        // Act
        GetUkrlpQueryResult sut = providerDetails;

        // Assert
        sut.LegalName.Should().Be("TestName1");
        sut.TradingName.Should().BeNull();
        sut.CharityNumber.Should().BeNull();
        sut.CompanyNumber.Should().BeNull();
    }
}