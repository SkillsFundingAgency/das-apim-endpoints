using MediatR;
using SFA.DAS.AdminRoatp.InnerApi.Responses;

namespace SFA.DAS.AdminRoatp.Application.Queries.GetProvidersAllowedToDeliverCourse;

public record GetProvidersAllowedToDeliverCourseQuery(string larsCode) : IRequest<RestrictedCourseDetailsModel>;