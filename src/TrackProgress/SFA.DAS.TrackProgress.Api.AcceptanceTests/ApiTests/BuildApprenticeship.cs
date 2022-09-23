using AutoFixture;
using SFA.DAS.TrackProgress.Api.AcceptanceTests.TestModels;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using SFA.DAS.TrackProgress.Apis.CoursesInnerApi;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using static SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi.GetApprenticeshipsResponse;

namespace SFA.DAS.TrackProgress.Api.AcceptanceTests.ApiTests;

public static partial class MockApiExtensions
{
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

    public static TrackProgressApiFactory WithApprenticeship(
        this TrackProgressApiFactory factory, Apprenticeship apprenticeship)
    {
        var mockServer = factory.CommitmentsApi.MockServer;
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

        mockServer
            .Given(
                Request.Create()
                    .WithPath($"/api/apprenticeships/{apprenticeships.First().Id}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithBodyAsJson(singleMockResponse));

        return factory;
    }

    public static TrackProgressApiFactory WithoutApprenticeship(
        this TrackProgressApiFactory factory, Apprenticeship apprenticeship)
    {
        var mockServer = factory.CommitmentsApi.MockServer;
        var fixture = new Fixture();

        var mockResponse = fixture.Build<GetApprenticeshipsResponse>()
            .With(x => x.StatusCode, HttpStatusCode.OK)
            .With(x => x.TotalApprenticeshipsFound, 0)
            .Without(x => x.Apprenticeships).Create();

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

        return factory;
    }

    public static TrackProgressApiFactory WithCourse(
        this TrackProgressApiFactory factory, Course course)
    {
        var mockServer = factory.CommitmentsApi.MockServer;
        var fixture = new Fixture();

        var courseOptionsResponse = fixture
                  .Build<GetCourseOptionsResponse>()
                  .With(x => x.Options, course.Options.ToList())
                  .Create();

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
            mockServer
                .Given(
                    Request.Create()
                        .WithPath($"/api/courses/standards/{course.Standard}/options/{option}/ksbs")
                        .UsingGet())
                .RespondWith(
                    Response.Create()
                        .WithBodyAsJson(content));
        }

        return factory;
    }
}