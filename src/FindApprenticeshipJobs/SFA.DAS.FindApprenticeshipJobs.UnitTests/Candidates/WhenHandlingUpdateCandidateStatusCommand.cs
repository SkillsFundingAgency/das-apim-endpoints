﻿using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Application.Commands.Candidates;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Requests;
using SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.UnitTests.Candidates
{
    public class WhenHandlingUpdateCandidateStatusCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Put_Is_Sent_And_Data_Returned(
            string govUkId,
            UpdateCandidateStatusCommand command,
            GetCandidateApiResponse getCandidateApiResponse,
            PutCandidateApiResponse putCandidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            UpdateCandidateStatusCommandHandler handler)
        {
            command.GovUkIdentifier = govUkId;
            command.Email = getCandidateApiResponse.Email;
            var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
            mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(getCandidateApiResponse, HttpStatusCode.OK, string.Empty));


            var expectedPutData = new PutCandidateApiRequestData
            {
                Email = command.Email,
                Status = command.Status
            };

            var expectedRequest = new PutCandidateApiRequest(getCandidateApiResponse.Id, expectedPutData);

            mockApiClient
                .Setup(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                    It.Is<PutCandidateApiRequest>(c =>
                        c.PutUrl == expectedRequest.PutUrl
                        && ((PutCandidateApiRequestData)c.Data).Email == command.Email)))
                .ReturnsAsync(new ApiResponse<PutCandidateApiResponse>(putCandidateApiResponse, HttpStatusCode.OK, string.Empty));


            await handler.Handle(command, CancellationToken.None);

            mockApiClient.Verify(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(c =>
                    c.PutUrl == expectedRequest.PutUrl
                    && ((PutCandidateApiRequestData)c.Data).Email == command.Email)), Times.Once());
        }

        [Test, MoqAutoData]
        public async Task And_Api_Returns_Null_Then_Put_Never_Called(
            string govUkId,
        UpdateCandidateStatusCommand command,
            GetCandidateApiResponse getCandidateApiResponse,
        PutCandidateApiResponse putCandidateApiResponse,
        [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            UpdateCandidateStatusCommandHandler handler)
        {
            command.GovUkIdentifier = govUkId;
            command.Email = getCandidateApiResponse.Email;
            var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
            mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));


            var expectedPutData = new PutCandidateApiRequestData
            {
                Email = command.Email,
                Status = command.Status
            };

            var expectedRequest = new PutCandidateApiRequest(getCandidateApiResponse.Id, expectedPutData);

            await handler.Handle(command, CancellationToken.None);

            mockApiClient.Verify(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(c =>
                    c.PutUrl == expectedRequest.PutUrl
                    && ((PutCandidateApiRequestData)c.Data).Email == command.Email)), Times.Never());
        }

        [Test, MoqAutoData]
        public async Task And_Api_Returns_Different_Email_Then_Put_Never_Called(
            string govUkId,
            UpdateCandidateStatusCommand command,
            GetCandidateApiResponse getCandidateApiResponse,
            PutCandidateApiResponse putCandidateApiResponse,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            UpdateCandidateStatusCommandHandler handler)
        {
            command.GovUkIdentifier = govUkId;
            var expectedGetCandidateRequest = new GetCandidateApiRequest(govUkId);
            mockApiClient.Setup(x => x.GetWithResponseCode<GetCandidateApiResponse>(
                    It.Is<GetCandidateApiRequest>(r => r.GetUrl == expectedGetCandidateRequest.GetUrl)))
                .ReturnsAsync(new ApiResponse<GetCandidateApiResponse>(null!, HttpStatusCode.NotFound, string.Empty));


            var expectedPutData = new PutCandidateApiRequestData
            {
                Email = command.Email,
                Status = command.Status
            };

            var expectedRequest = new PutCandidateApiRequest(getCandidateApiResponse.Id, expectedPutData);

            await handler.Handle(command, CancellationToken.None);

            mockApiClient.Verify(client => client.PutWithResponseCode<PutCandidateApiResponse>(
                It.Is<PutCandidateApiRequest>(c =>
                    c.PutUrl == expectedRequest.PutUrl
                    && ((PutCandidateApiRequestData)c.Data).Email == command.Email)), Times.Never());
        }
    }
}
