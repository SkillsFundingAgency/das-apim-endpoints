using System.Net;
using System.Text;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Tests;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");
            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForSandboxMode, isSandbox);

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(new
                {
                    TotalApprenticeshipsFound = 0,
                })
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_called_with_start_date_other_than_01()
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

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", BuildValidProgressDtoContent());

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

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
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "12345");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

            response.Should().Be400BadRequest();
        }
    }

    private static HttpRequestInterceptionBuilder CommitmentsApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private GetApprenticeshipsResponse GetMockApprenticeshipResponse(
        int numberFound, DeliveryModel deliveryModel = DeliveryModel.PortableFlexiJob, ApprenticeshipStatus status = ApprenticeshipStatus.Live)
    {
        var apprenticeships = new List<ApprenticeshipDetails>()
            {
                fixture.Build<ApprenticeshipDetails>()
                    .With(x => x.DeliveryModel, deliveryModel)
                    .With(x => x.ApprenticeshipStatus, status)
                    .Create()
            };

        if (numberFound > 1)
            apprenticeships.Add(fixture.Build<ApprenticeshipDetails>()
                    .With(x => x.DeliveryModel, deliveryModel)
                    .With(x => x.ApprenticeshipStatus, status)
                    .Create());

        var apprenticeshipsResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, numberFound)
            .With(x => x.Apprenticeships, apprenticeships).Create();

        return apprenticeshipsResponse;
    }

    private StringContent BuildValidProgressDtoContent()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
                {Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 5},
                    new() {Id = Guid.NewGuid().ToString(), Value = 6}
                }
            }
        };
        return new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
    }

}
