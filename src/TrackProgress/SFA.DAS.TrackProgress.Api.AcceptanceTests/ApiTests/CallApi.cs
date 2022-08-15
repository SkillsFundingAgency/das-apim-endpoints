using FluentAssertions;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Tests;
using System.Text;
using System.Text.Json;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ApiTests;

public class CallApi : ApiFixture
{
    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_no_sandbox_mode()
    {
        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(new
                {
                    TotalApprenticeshipsFound = 1,
                })
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.HeaderForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be201Created();
        }
    }

    [TestCase(null, true)]
    [TestCase("true", false)]
    [TestCase("false", true)]
    [TestCase("rubbish", true)]
    public async Task Track_progress_with_single_matching_apprenticeship_and_different_sandbox_modes(string? isSandbox, bool updateExpected)
    {
        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(new
                {
                    TotalApprenticeshipsFound = 1,
                })
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.HeaderForProviderId, "12345");
            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.HeaderForSandboxMode, isSandbox);
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            // TODO Ensure an update is called only when it is expected 

            response.Should().Be201Created();
        }
    }



    [Test]
    public async Task Track_progress_with_no_matching_apprenticeship()
    {
        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(new
                {
                    TotalApprenticeshipsFound = 0,
                })
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.HeaderForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be404NotFound();
        }
    }

    private static HttpRequestInterceptionBuilder CommitmentsApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();
}