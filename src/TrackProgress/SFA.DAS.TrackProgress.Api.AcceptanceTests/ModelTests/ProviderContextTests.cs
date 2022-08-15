using FluentAssertions;
using SFA.DAS.TrackProgress.Application.Models;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ModelTests;

public class ProviderContextTests
{
	[Test]
	public void Parse_a_valid_ukprn()
		=> ProviderContext.Create("123456", null).ProviderId.Should().Be(123456);

    [TestCase("true", true)]
    [TestCase("false", false)]
    [TestCase("", false)]
    [TestCase("XYZ", false)]
    [TestCase(null, false)]
    public void Parse_a_valid_ukprn_and_different_sandbox_settings(string? isSandbox, bool expectedMode)
        => ProviderContext.Create("123456", isSandbox).InSandboxMode.Should().Be(expectedMode);

    [TestCase(null)]
    [TestCase("")]
    [TestCase("invalid")]
    [TestCase("9223372036854775808")]
    [TestCase("-1")]
    public void Parse_an_invalid_ukprn(string input)
        => this.Invoking(_ => ProviderContext.Create(input, null)).Should().Throw<InvalidUkprnException>();
}
