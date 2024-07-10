﻿using System.Net;
using AutoFixture.NUnit3;
using Microsoft.AspNetCore.JsonPatch;
using Moq;
using Newtonsoft.Json;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.UpdateQualifications;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    [TestFixture]
    public class WhenHandlingUpdateQualificationsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Application_Status_Is_Updated(
            UpdateQualificationsCommand command,
            Domain.Models.Application candidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            UpdateQualificationsCommandHandler handler)
        {
            var expectedPatchRequest = new PatchApplicationApiRequest(command.ApplicationId, command.CandidateId, new JsonPatchDocument<Domain.Models.Application>());

            candidateApiClient
                .Setup(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)))
                .ReturnsAsync(new ApiResponse<string>(JsonConvert.SerializeObject(candidateApiResponse), HttpStatusCode.OK, string.Empty));

            await handler.Handle(command, CancellationToken.None);

            candidateApiClient
                .Verify(client => client.PatchWithResponseCode(It.Is<PatchApplicationApiRequest>(r => r.PatchUrl == expectedPatchRequest.PatchUrl)), Times.Once);
        }
    }
}
