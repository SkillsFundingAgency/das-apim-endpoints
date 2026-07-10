using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersNotAllowedToDeliverCourse;

public record GetProvidersNotAllowedToDeliverCourseQuery(string larsCode) : IRequest<RestrictedCourseDetailsModel>;
