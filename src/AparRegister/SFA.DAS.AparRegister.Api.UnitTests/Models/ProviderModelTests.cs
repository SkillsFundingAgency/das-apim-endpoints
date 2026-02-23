using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.AparRegister.Api.Models;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.AparRegister.Api.UnitTests.Models;

public class ProviderModelTests
{
    [Test]
    public void Operator_ConvertsFromOrganisationResponse()
    {
        var source = new OrganisationResponse
        {
            Ukprn = 12345678,
            LegalName = "Test Provider",
            TradingName = "Test Trading Name",
            Status = OrganisationStatus.Active,
            ProviderType = ProviderType.Main
        };
        ProviderModel result = source;

        using (new AssertionScope())
        {
            result.Ukprn.Should().Be(source.Ukprn);
            result.Name.Should().Be(source.LegalName);
            result.TradingName.Should().Be(source.TradingName);
            result.StatusId.Should().Be((int)source.Status);
            result.ProviderTypeId.Should().Be((int)source.ProviderType);
        }
    }

    [TestCase(ProviderType.Main, true)]
    [TestCase(ProviderType.Employer, true)]
    [TestCase(ProviderType.Supporting, false)]
    public void CanAccessApprenticeshipService_ReturnsExpectedResult(ProviderType providerType, bool expected)
    {
        var model = new ProviderModel
        {
            ProviderTypeId = (int)providerType,
            StatusId = (int)OrganisationStatus.Active
        };
        model.CanAccessApprenticeshipService.Should().Be(expected);
    }
}
