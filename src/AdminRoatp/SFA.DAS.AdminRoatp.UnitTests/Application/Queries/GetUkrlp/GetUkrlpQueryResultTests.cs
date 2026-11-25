using FluentAssertions;
using SFA.DAS.AdminRoatp.Application.Queries.GetUkrlp;
using SFA.DAS.AdminRoatp.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Roatp;

namespace SFA.DAS.AdminRoatp.UnitTests.Application.Queries.GetUkrlp;

public class GetUkrlpQueryResultTests
{
    [Test]
    public void ImplicitConversion_AllDataPresent_MapsAllDataCorrectly()
    {
        // Arrange
        var providerDetails = new UkrlpProviderModel
        {
            LegalName = "TestName1",
            TradingName = "TestAlias1",
            VerificationDetails = new List<VerificationInfo>
            {
                new(VerificationAuthority.CharityCommission, "12345", false),
                new(VerificationAuthority.CompaniesHouse, "67890", false)
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

    [TestCase(VerificationAuthority.CharityCommission, "12345", "12345", null)]
    [TestCase(VerificationAuthority.CompaniesHouse, "67890", null, "67890")]
    [TestCase("Test Authority", "54321", null, null)]
    [TestCase(null, null, null, null)]
    public void ImplicitConversion_VariationOfVerificationDetails_MapsCharityAndCompanyNumberCorrectly(string? verificationAuthority, string? verificationId, string? expectedCharityNumber, string? expectedCompanyNumber)
    {
        // Arrange
        var providerDetails = new UkrlpProviderModel
        {
            LegalName = "TestName1",
            TradingName = "TestAlias1",
            VerificationDetails = new List<VerificationInfo>
            {
                new(verificationAuthority, verificationId, false)
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
        var providerDetails = new UkrlpProviderModel
        {
            LegalName = null,
            TradingName = null,
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
        var providerDetails = new UkrlpProviderModel
        {
            LegalName = "TestName1",
            TradingName = null,
            VerificationDetails = new List<VerificationInfo>()
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