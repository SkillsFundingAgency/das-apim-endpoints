using AutoFixture;
using JustEat.HttpClientInterception;
using SFA.DAS.TrackProgress.Apis.CommitmentsV2InnerApi;
using System.Net;
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
    public static BuildApprenticeship WithApprenticeship(
        this HttpRequestInterceptionBuilder builder, TestModels.Apprenticeship apprenticeship)
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

        var query = $"providerid={apprenticeship.ProviderId}&searchterm={apprenticeship.Uln}&startdate={apprenticeship.StartDate.Year}-{apprenticeship.StartDate.Month:00}-{apprenticeship.StartDate.Day:00}";
        builder
           .ForPath("api/apprenticeships")
           .ForQuery(query)
           .Responds()
           .WithSystemTextJsonContent(mockResponse);

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

        return new BuildApprenticeship(builder, builder2);
    }

    public static BuildApprenticeship WithoutApprenticeship(
        this HttpRequestInterceptionBuilder builder, TestModels.Apprenticeship apprenticeship)
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

        return new BuildApprenticeship(builder);
    }
}