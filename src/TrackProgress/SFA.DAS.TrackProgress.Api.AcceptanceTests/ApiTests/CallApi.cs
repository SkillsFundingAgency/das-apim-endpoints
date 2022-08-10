using FluentAssertions;
using SFA.DAS.TrackProgress.Tests;
using System.Text;
using System.Text.Json;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ApiTests;

public class CallApi : ApiFixture
{
	[Test]
	public async Task Can_call_API()
	{
		var response = await client.GetAsync("/");
		response.Should().Be404NotFound();
	}

	[Test]
	public async Task Can_call_progress_api()
	{
		client.DefaultRequestHeaders.Add("x-request-context-subscription-name", "12345");
		var postData = new { Uln = 1, PlannedStartDate = "2022-02-20" };
		var content = new StringContent(
			JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

		var response = await client.PostAsync(
			$"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

		response.Should().Be200Ok();
	}
}
