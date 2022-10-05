using SFA.DAS.TrackProgressInternal.Api.AcceptanceTests.TestModels;
using System.Net;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace SFA.DAS.TrackProgressInternal.Api.AcceptanceTests;

public static class ApiDataBuilder
{
    public static ApiFactory Reset(this ApiFactory factory)
    {
        factory.InnerApis.Reset();
        factory.TrackProgressInnerApi.Reset();
        return factory;
    }

    public static ApiFactory WithApprenticeship(
        this ApiFactory factory, Apprenticeship apprenticeship)
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

    public static ApiFactory WithCourse(
        this ApiFactory factory, Course course)
    {
        factory.InnerApis
            .Given(
                Request.Create()
                    .WithPath($"/api/courses/standards/{course.Standard}")
                    .UsingGet())
            .RespondWith(
                Response.Create()
                    .WithStatusCode(HttpStatusCode.OK)
                    .WithBodyAsJson(new
                    {
                        course.Ksbs
                    }));

        return factory;
    }
}