using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Extensions;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations
{
    public class UpdateEmploymentLocationsCommandHandler(ICandidateApiClient<CandidateApiConfiguration> candidateApiClient, ILogger<UpdateEmploymentLocationsCommandHandler> logger) 
        : IRequestHandler<UpdateEmploymentLocationsCommand, UpdateEmploymentLocationsCommandResult>
    {
        public async Task<UpdateEmploymentLocationsCommandResult> Handle(UpdateEmploymentLocationsCommand command, CancellationToken cancellationToken)
        {
            var jsonPatchDocument = new JsonPatchDocument<Domain.Models.Application>();
            jsonPatchDocument.Replace(x => x.EmploymentLocationStatus, command.EmploymentLocationSectionStatus);

            var patchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, jsonPatchDocument);
            var patchResult = await candidateApiClient.PatchWithResponseCode(patchRequest);
            if (patchResult.StatusCode != System.Net.HttpStatusCode.OK)
            {
                logger.LogError("Unable to patch application for candidate Id {CandidateId}", command.CandidateId);
                throw new HttpRequestContentException($"Unable to patch application for candidate Id {command.CandidateId}", patchResult.StatusCode, patchResult.ErrorContent);
            }

            var request = new PutUpsertEmploymentLocationsApiRequest(command.ApplicationId, command.CandidateId, command.EmployerLocation.Id, new PutUpsertEmploymentLocationsApiRequest.PutUpsertEmploymentLocationsApiRequestData
            {
                Addresses = command.EmployerLocation.Addresses,
                EmployerLocationOption = (short)command.EmployerLocation.EmployerLocationOption!,
                EmploymentLocationInformation = command.EmployerLocation.EmploymentLocationInformation,
            });

            var upsertResult = await candidateApiClient.PutWithResponseCode<PutUpsertEmploymentLocationsApiResponse>(request);
            upsertResult.EnsureSuccessStatusCode();

            if (upsertResult is null) return null;

            return new UpdateEmploymentLocationsCommandResult
            {
                Application = JsonConvert.DeserializeObject<Domain.Models.Application>(patchResult.Body),
                Id = upsertResult.Body.Id
            };
        }
    }
}