using MediatR;

namespace SFA.DAS.RoatpCourseManagement.Application.ProviderCourseForecasts.Queries.GetProviderCourseForecasts;

public record GetProviderCourseForecastsQuery(int Ukprn, string LarsCode) : IRequest<GetProviderCourseForecastsQueryResult>;
