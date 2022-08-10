using FluentAssertions;
using SFA.DAS.TrackProgress.Application.Models;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ModelTests;

public class UkprnTests
{
	[Test]
	public void Parse_a_valid_ukprn()
		=> Ukprn.Parse("123456").Should().BeEquivalentTo(new { Value = 123456 });

	[TestCase(null)]
	[TestCase("")]
	[TestCase("invalid")]
	[TestCase("9223372036854775808")]
	[TestCase("-1")]
	public void Parse_an_invalid_ukprn(string input)
		=> Ukprn.Parse(input).Should().BeNull();

}
