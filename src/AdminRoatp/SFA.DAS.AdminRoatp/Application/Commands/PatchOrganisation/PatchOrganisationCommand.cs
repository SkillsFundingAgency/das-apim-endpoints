using System.Net;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using SFA.DAS.AdminRoatp.InnerApi.Requests;

namespace SFA.DAS.AdminRoatp.Application.Commands.PatchOrganisation;
public record PatchOrganisationCommand(int Ukprn, string UserId, JsonPatchDocument<PatchOrganisationModel> PatchDoc) : IRequest<HttpStatusCode>;
