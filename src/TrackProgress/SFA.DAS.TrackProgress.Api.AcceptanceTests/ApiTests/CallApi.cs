using System.Net;
using System.Text;
using System.Text.Json;
using AutoFixture;
using FluentAssertions;
using JustEat.HttpClientInterception;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Apis.CoursesInnerApi;
using SFA.DAS.TrackProgress.Application.DTOs;
using SFA.DAS.TrackProgress.Tests;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;

public class CallApi : ApiFixture
{
    [TestCase("OPTION")]
    [TestCase("")]
    [TestCase(null)]
    [TestCase(" ")]
    public async Task Track_progress_with_single_matching_apprenticeship_and_no_sandbox_mode(string optionToTest)
    {
        var mockResponse = GetMockApprenticeshipResponse(1);
        var singleMockResponse = GetMockSingleApprenticeshipResponse(DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live,  "STDUID", optionToTest);
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();
        var courseOptionsResponse = fixture.Build<GetCourseOptionsResponse>().With(x => x.Options, new List<string>()).Create();
        var optionCode = string.IsNullOrWhiteSpace(singleMockResponse.Option) ? "core" : singleMockResponse.Option; 
        var validDto = BuildValidProgressDtoContentFromCourseResponse(courseKsbsResponse);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{optionCode}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            CoursesApi2
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}")
                .Responds()
                .WithSystemTextJsonContent(courseOptionsResponse)
                .RegisterWith(Interceptor);

            TrackProgressInnerApi
                .ForPost()
                .ForPath($"apprentice/{12345}/{singleMockResponse.Uln}/2021-01/progress")
                .Responds()
                .WithStatus(HttpStatusCode.Created)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

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
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();
        var validDto = BuildValidProgressDtoContentFromCourseResponse(courseKsbsResponse);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            TrackProgressInnerApi
                .ForPost()
                .ForPath($"apprentice/{12345}/{singleMockResponse.Uln}/2021-01-01/progress")
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");
            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForSandboxMode, isSandbox);

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

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

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

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

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

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

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01-19"}/progress", BuildValidProgressDtoContent());

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_with_non_portable_flexi_delivery_model()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.Regular);
        var singleMockResponse = GetMockSingleApprenticeshipResponse(DeliveryModel.Regular);
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_with_non_started_apprenticeship()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.WaitingToStart);
        var singleMockResponse = GetMockSingleApprenticeshipResponse(DeliveryModel.FlexiJobAgency, ApprenticeshipStatus.WaitingToStart);
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildValidProgressDtoContent());

            response.Should().Be400BadRequest();
        }
    }

    [Test]
    public async Task Track_progress_with_invalid_Ids()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithInvalidIds());

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("Failed to record progress due to one or more validation errors");
        }
    }

    [Test]
    public async Task Track_progress_with_invalid_Values()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithInvalidValues());

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("Failed to record progress due to one or more validation errors");
        }
    }

    [Test]
    public async Task Track_progress_with_no_Ksbs()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithNoKsbs());

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("KSBs are required");
        }
    }

    [Test]
    public async Task Track_progress_with_null_Ids()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithANullId());

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("Failed to record progress due to one or more validation errors");
        }
    }

    [Test]
    public async Task Track_progress_with_duplicate_Ids()
    {
        var mockResponse = GetMockApprenticeshipResponse(1, DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", BuildProgressDtoContentWithDuplicateIds());

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("Failed to record progress due to one or more validation errors");
        }
    }

    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_a_subset_of_valid_ksbs()
    {
        var mockResponse = GetMockApprenticeshipResponse(1);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();
        var validDto = BuildValidSubsetProgressDtoContentFromCourseResponse(courseKsbsResponse);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            TrackProgressInnerApi
                .ForPost()
                .ForPath($"/apprentice/{12345}/{singleMockResponse.Uln}/2021-01/progress")
                .Responds()
                .WithSystemTextJsonContent("{}")
                .WithStatus(HttpStatusCode.Created)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

            response.Should().Be201Created();
        }
    }

    [Test]
    public async Task Track_progress_with_single_matching_apprenticeship_and_an_additional_ksbs_which_doesnt_belong_course()
    {
        var mockResponse = GetMockApprenticeshipResponse(1);
        var singleMockResponse = GetMockSingleApprenticeshipResponse();
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();
        var validDto = BuildAdditionalKsbToProgressDtoContentFromCourseResponse(courseKsbsResponse);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem.Title.Should().StartWith("Failed to record progress due to one or more validation errors");
        }
    }

    [Test]
    public async Task Track_progress_with_no_options_but_options_do_exist_for_course()
    {
        var mockResponse = GetMockApprenticeshipResponse(1);
        var singleMockResponse = GetMockSingleApprenticeshipResponse(DeliveryModel.PortableFlexiJob, ApprenticeshipStatus.Live, "STDUID", "");
        var courseKsbsResponse = fixture.Create<GetKsbsForCourseOptionResponse>();
        var courseOptionsResponse = fixture.Build<GetCourseOptionsResponse>().With(x => x.Options, new List<string>() { "Option 1", "Option 2" }).Create();
        var validDto = BuildAdditionalKsbToProgressDtoContentFromCourseResponse(courseKsbsResponse);

        using (Interceptor.BeginScope())
        {
            CommitmentsApi
                .ForPath("api/apprenticeships")
                .ForQuery("providerid=12345&searchterm=1&startdate=2020-01-01")
                .Responds()
                .WithSystemTextJsonContent(mockResponse)
                .RegisterWith(Interceptor);

            CommitmentsApi2
                .ForPath("api/apprenticeships/1")
                .Responds()
                .WithSystemTextJsonContent(singleMockResponse)
                .RegisterWith(Interceptor);

            CoursesApi
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}/options/{singleMockResponse.Option}/ksbs")
                .Responds()
                .WithSystemTextJsonContent(courseKsbsResponse)
                .RegisterWith(Interceptor);

            CoursesApi2
                .ForPath($"/api/courses/standards/{singleMockResponse.StandardUId}")
                .Responds()
                .WithSystemTextJsonContent(courseOptionsResponse)
                .RegisterWith(Interceptor);

            client.DefaultRequestHeaders.Add(SubscriptionHeaderConstants.ForProviderId, "Provider-12345-TrackProgressOuterApi");

            var response = await client.PostAsync(
                $"/apprenticeships/{1}/{"2020-01"}/progress", validDto);

            response.Should().Be400BadRequest();
            var body = await response.Content.ReadAsStringAsync();
            var problem = JsonSerializer.Deserialize<ProblemDetails>(body);
            problem?.Title.Should().StartWith("This apprenticeship requires an option to be set to record progress against it");
        }
    }

    private static HttpRequestInterceptionBuilder CommitmentsApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private static HttpRequestInterceptionBuilder CommitmentsApi2 { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private static HttpRequestInterceptionBuilder CoursesApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private static HttpRequestInterceptionBuilder CoursesApi2 { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private static HttpRequestInterceptionBuilder TrackProgressInnerApi { get; } =
        new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost();

    private GetApprenticeshipsResponse GetMockApprenticeshipResponse(
        int numberFound, DeliveryModel deliveryModel = DeliveryModel.PortableFlexiJob, ApprenticeshipStatus status = ApprenticeshipStatus.Live)
    {
        var apprenticeships = new List<ApprenticeshipDetails>()
            {
                fixture.Build<ApprenticeshipDetails>()
                    .With(x=>x.Id, 1)
                    .With(x => x.DeliveryModel, deliveryModel)
                    .With(x => x.ApprenticeshipStatus, status)
                    .Create()
            };

        if (numberFound > 1)
            apprenticeships.Add(fixture.Build<ApprenticeshipDetails>()
                .With(x => x.Id, 2)
                .With(x => x.DeliveryModel, deliveryModel)
                .With(x => x.ApprenticeshipStatus, status)
                .Create());

        var apprenticeshipsResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, numberFound)
            .With(x => x.Apprenticeships, apprenticeships).Create();

        return apprenticeshipsResponse;
    }

    private GetApprenticeshipResponse GetMockSingleApprenticeshipResponse(DeliveryModel deliveryModel = DeliveryModel.PortableFlexiJob, ApprenticeshipStatus status = ApprenticeshipStatus.Live, string standardUId = "STD", string option = "OPTION1")
    {
        return fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.Id, 1)
            .With(x=>x.StandardUId, standardUId)
            .With(x=>x.Option, option)
            .With(x => x.DeliveryModel, deliveryModel)
            .With(x => x.ApprenticeshipStatus, status)
            .Create();
    }

    private GetKsbsForCourseOptionResponse BuildMockCourseResponseFrom(List<CourseKsb> ksbs)
    {
        return fixture.Build<GetKsbsForCourseOptionResponse>()
            .With(x => x.Ksbs, ksbs)
            .Create();
    }

    private StringContent BuildValidProgressDtoContentFromCourseResponse(GetKsbsForCourseOptionResponse response)    
    {
        var ksbs = response.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).ToList();
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildValidSubsetProgressDtoContentFromCourseResponse(GetKsbsForCourseOptionResponse response)
    {
        var ksbs = response.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).Take(2).ToList();
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildAdditionalKsbToProgressDtoContentFromCourseResponse(GetKsbsForCourseOptionResponse response)
    {
        var ksbs = response.Ksbs.Select(x => new ProgressDto.Ksb { Id = x.Id.ToString(), Value = 5 }).Take(2).ToList();
        ksbs.Add(new ProgressDto.Ksb { Id = Guid.NewGuid().ToString(), Value = 9});
        var dto = new ProgressDto { Progress = new ProgressDto.ProgressDetails { Ksbs = ksbs } };
        var validDto = BuildProgressDtoContent(dto);

        return BuildProgressDtoContent(dto);
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
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithInvalidIds()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = "XXXX", Value = 5},
                    new() {Id = "123", Value = 6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithInvalidValues()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 101},
                    new() {Id = Guid.NewGuid().ToString(), Value = -6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithNoKsbs()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>()
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithANullId()
    {
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = null, Value = 5},
                    new() {Id = Guid.NewGuid().ToString(), Value = 6}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }

    private StringContent BuildProgressDtoContentWithDuplicateIds()
    {
        var id = Guid.NewGuid().ToString();
        var dto = new ProgressDto
        {
            Progress = new ProgressDto.ProgressDetails
            {
                Ksbs = new List<ProgressDto.Ksb>
                {
                    new() {Id = Guid.NewGuid().ToString(), Value = 5},
                    new() {Id = id, Value = 33},
                    new() {Id = id, Value = 60}
                }
            }
        };
        return BuildProgressDtoContent(dto);
    }


    private StringContent BuildProgressDtoContent(ProgressDto dto)
    {
        return new StringContent(JsonSerializer.Serialize(dto), Encoding.UTF8, "application/json");
    }
}