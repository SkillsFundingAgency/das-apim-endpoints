using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateEmploymentLocations;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    [TestFixture]
    public class WhenHandlingUpdateEmploymentLocationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_EmploymentLocation_Is_Updated(
            UpdateEmploymentLocationsCommand command,
            PutUpsertEmploymentLocationsApiResponse apiResponse,
        FindAnApprenticeship.Domain.Models.Application updateApplicationResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
        UpdateEmploymentLocationsCommandHandler handler)
        {
            var expectedPostUpsertEmploymentLocationsApiRequest = new PutUpsertEmploymentLocationsApiRequest(command.ApplicationId, command.CandidateId, command.EmployerLocation.Id, new PutUpsertEmploymentLocationsApiRequest.PutUpsertEmploymentLocationsApiRequestData());

            candidateApiClient
                        .Setup(client => client.PutWithResponseCode<PutUpsertEmploymentLocationsApiResponse>(
                            It.Is<PutUpsertEmploymentLocationsApiRequest>(r => r.PutUrl == expectedPostUpsertEmploymentLocationsApiRequest.PutUrl)))
                        .ReturnsAsync(new ApiResponse<PutUpsertEmploymentLocationsApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<FindAnApprenticeship.Domain.Models.Application>());

            candidateApiClient
                    .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                    .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(updateApplicationResponse), HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Application.Should().BeEquivalentTo(updateApplicationResponse);
            }
        }

        [Test, MoqAutoData]
        public async Task Then_The_Update_Application_Status_Api_Response_NotFound_CommandResult_Is_Returned_As_Expected(
            UpdateEmploymentLocationsCommand command,
            PutUpsertEmploymentLocationsApiResponse apiResponse,
            FindAnApprenticeship.Domain.Models.Application updateApplicationResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            UpdateEmploymentLocationsCommandHandler handler)
        {
            var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<FindAnApprenticeship.Domain.Models.Application>());

            candidateApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>("", HttpStatusCode.BadRequest, string.Empty));

            Func<Task> act = async () => { await handler.Handle(command, CancellationToken.None); };

            await act.Should().ThrowAsync<HttpRequestContentException>();
        }
    }
}
