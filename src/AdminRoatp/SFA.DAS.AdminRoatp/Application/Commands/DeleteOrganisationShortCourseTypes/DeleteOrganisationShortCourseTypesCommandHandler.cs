using MediatR;
using SFA.DAS.AdminRoatp.Infrastructure;
using System.Net;

namespace SFA.DAS.AdminRoatp.Application.Commands.DeleteOrganisationShortCourseTypes;
public class DeleteOrganisationShortCourseTypesCommandHandler(IRoatpServiceRestApiClient _apiClient) : IRequestHandler<DeleteOrganisationShortCourseTypesCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(DeleteOrganisationShortCourseTypesCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _apiClient.DeleteShortCourseTypes(request.Ukprn, request.UserId, cancellationToken);
        return response.StatusCode;
    }
}