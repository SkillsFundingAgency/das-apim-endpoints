using AutoFixture;
using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;
using SFA.DAS.TrackProgressInternal.Apis.CoursesInnerApi;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.ApiTests;

public static class MockApiExtensions
{
    public static TrackProgressApiFactory Reset(this TrackProgressApiFactory factory)
    {
        factory.InnerApis.Reset();
        factory.TrackProgressInnerApi.Reset();
        factory.WithStubProviderResponse();
        return factory;
    }

    public static TrackProgressApiFactory WithStubProviderResponse(
        this TrackProgressApiFactory factory)
    {
        factory.TrackProgressInnerApi
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
        factory.TrackProgressInnerApi
            .Given(
                Request.Create()
                    .WithPath($"/apprenticeships/{apprenticeship.CommitmentsApprenticeshipId}/AggregateProgress")
                    .UsingPost()
                    .WithBody(x => true))
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK));

        return factory;
    }

    //public static TrackProgressApiFactory WithoutApprenticeship(
    //    this TrackProgressApiFactory factory, Apprenticeship apprenticeship)
    //{
    //    var fixture = new Fixture();

    //    var mockResponse = fixture.Build<GetApprenticeshipsResponse>()
    //        .With(x => x.StatusCode, HttpStatusCode.OK)
    //        .With(x => x.TotalApprenticeshipsFound, 0)
    //        .Without(x => x.Apprenticeships).Create();

    //    factory.InnerApis
    //        .Given(
    //            Request.Create()
    //                .WithPath("/api/apprenticeships")
    //                .WithParam("providerid", apprenticeship.ProviderId.ToString())
    //                .WithParam("searchterm", apprenticeship.Uln.ToString())
    //                .WithParam("startdate", $"{apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}")
    //                .UsingGet())
    //        .RespondWith(
    //            Response.Create()
    //                .WithBodyAsJson(mockResponse));

    //    return factory;
    //}

    public static TrackProgressApiFactory WithCourse(
        this TrackProgressApiFactory factory, Course course)
    {
        var fixture = new Fixture();

        var courseOptionsResponse = fixture
                  .Build<GetCourseOptionsResponse>()
                  .With(x => x.Options, course.Options.ToList())
                  .Create();

        factory.InnerApis
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
            factory.InnerApis
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