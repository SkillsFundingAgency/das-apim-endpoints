﻿using System.Net;
using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Apply.CreateJob;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands.Apply
{
    public class WhenHandlingCreateJobCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Job_Is_Created(
            CreateJobCommand command,
            PutUpsertWorkHistoryApiResponse apiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> candidateApiClient,
            CreateJobCommandHandler handler)
        {
            var expectedRequest = new PutUpsertWorkHistoryApiRequest(command.ApplicationId, command.CandidateId, Guid.NewGuid(), new PutUpsertWorkHistoryApiRequest.PutUpsertWorkHistoryApiRequestData());

            candidateApiClient
                        .Setup(client => client.PutWithResponseCode<PutUpsertWorkHistoryApiResponse>(
                            It.Is<PutUpsertWorkHistoryApiRequest>(r => r.PutUrl.StartsWith(expectedRequest.PutUrl.Substring(0, 86)))))
                        .ReturnsAsync(new ApiResponse<PutUpsertWorkHistoryApiResponse>(apiResponse, HttpStatusCode.OK, string.Empty));

            var actual = await handler.Handle(command, CancellationToken.None);

            using (new AssertionScope())
            {
                actual.Should().NotBeNull();
                actual.Id.Should().NotBeEmpty();
            }
        }
    }
}
