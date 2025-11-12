using MediatR;
using System.Net;

namespace SFA.DAS.AdminRoatp.Application.Commands.DeleteOrganisationShortCourseTypes;
public record class DeleteOrganisationShortCourseTypesCommand(int Ukprn, string UserId) : IRequest<HttpStatusCode>;