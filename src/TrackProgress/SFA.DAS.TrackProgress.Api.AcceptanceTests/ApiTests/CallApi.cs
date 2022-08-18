using AutoFixture;
using FluentAssertions;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Tests;
using System.Net;
using System.Text;
using System.Text.Json;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;

namespace SFA.DAS.TrackProgress.OuterApi.Tests.ApiTests;

public class CallApi : ApiFixture
{
    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_no_sandbox_mode()
    {
        var mockResponse = GetMockApprenticeshipResponse(1);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
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
        var mockResponse = GetMockApprenticeshipResponse(1);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForSandboxMode, isSandbox);
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

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be404NotFound();
        }
    }

    [Test]
    public async Task Track_progress_with_multiple_matching_apprenticeships()
    {
        var mockResponse = GetMockApprenticeshipResponse(2);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_with_non_portable_flexi_delivery_model()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.Regular);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_with_non_started_apprenticeship()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.WaitingToStart);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-19")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            var postData = new { };
            var content = new StringContent(
                JsonSerializer.Serialize(postData), Encoding.UTF8, "application/json");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", content);

            response.Should().Be400BadRequest();
        }
    }

    private static HttpRequestInterceptionBuilder CommitmentsApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private GetApprenticeshipsResponse GetMockApprenticeshipResponse(
        int numberFound, DeliveryModel deliveryModel = DeliveryModel.PortableFlexiJob, ApprenticeshipStatus status = ApprenticeshipStatus.Live)
    {
        var apprenticeships = new List<ApprenticeshipDetailsResponse>()
            {
                fixture.Build<ApprenticeshipDetailsResponse>()
                    .With(x => x.DeliveryModel, deliveryModel)
                    .With(x => x.ApprenticeshipStatus, status)
                    .Create()
            };

        if (numberFound > 1)
            apprenticeships.Add(fixture.Build<ApprenticeshipDetailsResponse>()
                    .With(x => x.DeliveryModel, deliveryModel)
                    .With(x => x.ApprenticeshipStatus, status)
                    .Create());

        var apprenticeshipsResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, numberFound)
            .With(x => x.Apprenticeships, apprenticeships).Create();

        return apprenticeshipsResponse;
    }
}
