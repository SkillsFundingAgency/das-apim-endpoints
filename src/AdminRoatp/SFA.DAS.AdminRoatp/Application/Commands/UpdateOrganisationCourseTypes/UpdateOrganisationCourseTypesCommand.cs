using MediatR;
using System.Net;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;
public record UpdateOrganisationCourseTypesCommand(int ukprn, int[] CourseTypeIds, string UserId) : IRequest<HttpStatusCode>;
