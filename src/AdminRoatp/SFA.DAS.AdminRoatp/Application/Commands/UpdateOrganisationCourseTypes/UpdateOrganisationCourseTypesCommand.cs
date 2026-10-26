using MediatR;

namespace SFA.DAS.AdminRoatp.Application.Commands.UpdateOrganisationCourseTypes;

public record UpdateOrganisationCourseTypesCommand(int Ukprn, int[] CourseTypeIds, string UserId, string UserDisplayName) : IRequest;