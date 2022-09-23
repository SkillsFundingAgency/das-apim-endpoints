using AutoFixture;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Apis.CoursesInnerApi;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;

public record BuildApprenticeship(params HttpRequestInterceptionBuilder[] Builders)
{
    public void RegisterWith(HttpClientInterceptorOptions interceptor)
    {
        foreach (var builder in Builders)
            builder.RegisterWith(interceptor);
    }
}

public static partial class MockApiExtensions
{
    public static TrackProgressApiFactory WithApprenticeship(
        this TrackProgressApiFactory factory, Apprenticeship apprenticeship)
    {
        new HttpRequestInterceptionBuilder()
            .Requests().ForHttps().ForAnyHost()
            .WithApprenticeship(apprenticeship, factory.CommitmentsApi.MockServer)
            .RegisterWith(factory.Interceptor);
        return factory;
    }

    public static TrackProgressApiFactory WithoutApprenticeship(
        this TrackProgressApiFactory factory, Apprenticeship apprenticeship)
    {
        new HttpRequestInterceptionBuilder()
            .Requests().ForHttps().ForAnyHost()
            .WithoutApprenticeship(apprenticeship, factory.CommitmentsApi.MockServer)
            .RegisterWith(factory.Interceptor);
        return factory;
    }

    public static TrackProgressApiFactory WithCourse(
        this TrackProgressApiFactory factory, Course course)
    {
        new HttpRequestInterceptionBuilder()
            .Requests().ForHttps().ForAnyHost()
            .WithCourse(course, factory.CommitmentsApi.MockServer)
            .RegisterWith(factory.Interceptor);
        return factory;
    }

    public static TrackProgressApiFactory Reset(this TrackProgressApiFactory factory)
    {
        factory.CommitmentsApi.MockServer.Reset();
        factory.WithStubProviderResponse();
        return factory;
    }

    public static TrackProgressApiFactory WithStubProviderResponse(
        this TrackProgressApiFactory factory)
    {
        factory.CommitmentsApi.MockServer
            .Given(
                Request.Create()
                    .UsingPost()
                    .WithPath("/progress"))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.Created)
                    .WithBodyAsJson(new object()));
        return factory;
    }

    public static BuildApprenticeship WithApprenticeship(
        this HttpRequestInterceptionBuilder builder, Apprenticeship apprenticeship, WireMockServer mockServer)
    {
        var fixture = new Fixture();

        var apprenticeships = Enumerable.Range(1, apprenticeship.NumberOfApprenticeships)
            .Select(_ => fixture.Build<ApprenticeshipDetails>()
            .With(x => x.DeliveryModel, apprenticeship.DeliveryModel)
            .With(x => x.ApprenticeshipStatus, apprenticeship.Status)
            .With(x => x.StartDate, apprenticeship.StartDate.ToDateTime(TimeOnly.MinValue))
            .With(x => x.StopDate, apprenticeship.StopDate?.ToDateTime(TimeOnly.MinValue))
            .Create())
            .ToList();

        var mockResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, 1)
            .With(x => x.Apprenticeships, apprenticeships).Create();

        //http://localhost:53736/api/apprenticeships?providerid=12345&searchterm=1&startdate=2020-01-01
        //                       api/apprenticeships?providerid=12345&searchterm=1&startdate=2020-01-01

        var query = $"providerid={apprenticeship.ProviderId}&searchterm={apprenticeship.Uln}&startdate={apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}";
        builder
           .ForPath("api/apprenticeships")
           .ForQuery(query)
           .Responds()
           .WithSystemTextJsonContent(mockResponse);
        mockServer
            .Given(
                Request.Create()
                    .WithPath("/api/apprenticeships")
                    .WithParam("providerid", apprenticeship.ProviderId.ToString())
                    .WithParam("searchterm", apprenticeship.Uln.ToString())
                    .WithParam("startdate", $"{apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(mockResponse));

        var singleMockResponse =
            fixture.Build<GetApprenticeshipResponse>()
            .With(x => x.Id, apprenticeships.First().Id)
            .With(x => x.Uln, apprenticeship.Uln.ToString())
            .With(x => x.StandardUId, apprenticeship.Standard)
            .With(x => x.Option, apprenticeship.Option)
            .With(x => x.DeliveryModel, apprenticeship.DeliveryModel)
            .With(x => x.ApprenticeshipStatus, apprenticeship.Status)
            .With(x => x.StartDate, apprenticeship.StartDate.ToDateTime(TimeOnly.MinValue))
            .With(x => x.StopDate, apprenticeship.StopDate?.ToDateTime(TimeOnly.MinValue))
            .Create();

        var builder2 = new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost()
            .ForPath($"api/apprenticeships/{apprenticeships.First().Id}")
            .Responds()
            .WithSystemTextJsonContent(singleMockResponse);
        mockServer
            .Given(
                Request.Create()
                    .WithPath($"/api/apprenticeships/{apprenticeships.First().Id}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(singleMockResponse));

        return new BuildApprenticeship(builder, builder2);
    }

    public static BuildApprenticeship WithoutApprenticeship(
        this HttpRequestInterceptionBuilder builder, Apprenticeship apprenticeship, WireMockServer mockServer)
    {
        var fixture = new Fixture();

        var mockResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, 0)
            .Without(x => x.Apprenticeships).Create();

        var query = $"providerid={apprenticeship.ProviderId}&searchterm={apprenticeship.Uln}&startdate={apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}";
        builder
           .ForPath("api/apprenticeships")
           .ForQuery(query)
           .Responds()
           .WithSystemTextJsonContent(mockResponse);
        mockServer
            .Given(
                Request.Create()
                    .WithPath("/api/apprenticeships")
                    .WithParam("providerid", apprenticeship.ProviderId.ToString())
                    .WithParam("searchterm", apprenticeship.Uln.ToString())
                    .WithParam("startdate", $"{apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(mockResponse));

        return new BuildApprenticeship(builder);
    }

    public static BuildApprenticeship WithCourse(
        this HttpRequestInterceptionBuilder _, Course course, WireMockServer mockServer)
    {
        var fixture = new Fixture();

        var builders = new List<HttpRequestInterceptionBuilder> { };

        var courseOptionsResponse = fixture
                  .Build<GetCourseOptionsResponse>()
                  .With(x => x.Options, course.Options.ToList())
                  .Create();

        builders.Add(
            new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost()
                .ForPath($"/api/courses/standards/{course.Standard}")
                .Responds()
                .WithSystemTextJsonContent(courseOptionsResponse));
        mockServer
            .Given(
                Request.Create()
                    .WithPath($"/api/courses/standards/{course.Standard}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(courseOptionsResponse));

        foreach (var option in course.CoreAndOptions)
        {
            var content = fixture
                .Build<GetKsbsForCourseOptionResponse>()
                .With(x => x.Ksbs,
                            course.Ksbs.Select(k =>
                                new CourseKsb { Type = k.Type, Id = k.Id }).ToList())
                .Create();
            builders.Add(
                new HttpRequestInterceptionBuilder().Requests().ForHttps().ForAnyHost()
                    .ForPath($"/api/courses/standards/{course.Standard}/options/{option}/ksbs")
                    .Responds()
                    .WithSystemTextJsonContent(content));
            mockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/standards/{course.Standard}/options/{option}/ksbs")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(content));
        }

        return new BuildApprenticeship(builders.ToArray());
    }
}