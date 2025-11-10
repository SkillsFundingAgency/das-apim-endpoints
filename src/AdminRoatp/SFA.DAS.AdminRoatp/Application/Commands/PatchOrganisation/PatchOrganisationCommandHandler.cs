using System.Net;
using MediatR;
using SFA.DAS.AdminRoatp.Infrastructure;

namespace SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;

public class PatchOrganisationCommandHandler(IRoatpServiceRestApiClient _roatpServiceApiClient) : IRequestHandler<PatchOrganisationCommand, HttpStatusCode>
{
    public async Task<HttpStatusCode> Handle(PatchOrganisationCommand request, CancellationToken cancellationToken)
    {
        HttpResponseMessage response = await _roatpServiceApiClient.PatchOrganisation(request.Ukprn, request.UserId, request.PatchDoc, cancellationToken);
        return response.StatusCode;
    }
}
