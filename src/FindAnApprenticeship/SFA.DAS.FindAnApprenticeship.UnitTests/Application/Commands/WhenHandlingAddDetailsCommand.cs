﻿using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Requests;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.SharedOuterApi.Infrastructure;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users.AddDetails;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Commands
{
    public class WhenHandlingAddDetailsCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Put_Is_Sent_And_Data_Returned(
            AddDetailsCommand command,
            [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
            AddDetailsCommandHandler handler
            )
        {
            var expectedPutData = new PutCandidateApiRequestData
            {
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName
            };

            var expectedRequest = new PutCandidateApiRequest(command.CandidateId, expectedPutData);

            mockApiClient
                .Setup(client => client.PutWithResponseCode<NullResponse>(
                    It.Is<PutCandidateApiRequest>(c=>
                        c.PutUrl == expectedRequest.PutUrl
                        && ((PutCandidateApiRequestData)c.Data).FirstName == command.FirstName
                        && ((PutCandidateApiRequestData)c.Data).LastName == command.LastName
                        && ((PutCandidateApiRequestData)c.Data).Email == command.Email)))
                .ReturnsAsync(new ApiResponse<NullResponse>(null, HttpStatusCode.OK, string.Empty));

            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be(Unit.Value);
        }

        [Test, MoqAutoData]
        public void And_Api_Returns_Null_Then_Return_Null(
           AddDetailsCommand command,
           [Frozen] Mock<ICandidateApiClient<CandidateApiConfiguration>> mockApiClient,
           AddDetailsCommandHandler handler)
        {
            // Arrange
            mockApiClient.Setup(x => x.PutWithResponseCode<NullResponse>(It.IsAny<PutCandidateApiRequest>()))
                .ReturnsAsync(new ApiResponse<NullResponse>(new NullResponse(), HttpStatusCode.BadRequest, "error"));

            // Act
            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            // Assert
            act.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
