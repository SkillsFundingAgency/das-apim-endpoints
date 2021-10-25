﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.SetApplicationAcceptance;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.SetApplicationAcceptance
{
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task And_Acceptance_Is_Accept_And_Inner_Api_Method_Call_Success_Returns_True(
            SetApplicationAcceptanceCommand setApplicationAcceptanceCommand,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            SetApplicationAcceptanceCommandHandler setApplicationAcceptanceCommandHandler)
        {
            setApplicationAcceptanceCommand.Acceptance = Types.ApplicationAcceptance.Accept;

            Func<AcceptFundingRequest, CancellationToken, ApiResponse<AcceptFundingRequest>> acceptFundingProvider =
                (x, y) =>
                {
                    return new ApiResponse<AcceptFundingRequest>(
                        x,
                        System.Net.HttpStatusCode.NoContent,
                        null);
                };

            mockLevyTransferMatchingService
                .Setup(x => x.AcceptFunding(It.IsAny<AcceptFundingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(acceptFundingProvider);

            var result = await setApplicationAcceptanceCommandHandler.Handle(setApplicationAcceptanceCommand, CancellationToken.None);

            Assert.IsTrue(result);
        }

        [Test, MoqAutoData]
        public async Task And_Acceptance_Is_Accept_And_Inner_Api_Method_Call_Unsuccessful_Returns_False(
            SetApplicationAcceptanceCommand setApplicationAcceptanceCommand,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            SetApplicationAcceptanceCommandHandler setApplicationAcceptanceCommandHandler)
        {
            setApplicationAcceptanceCommand.Acceptance = Types.ApplicationAcceptance.Accept;

            Func<AcceptFundingRequest, CancellationToken, ApiResponse<AcceptFundingRequest>> acceptFundingProvider =
                (x, y) =>
                {
                    return new ApiResponse<AcceptFundingRequest>(
                        x,
                        System.Net.HttpStatusCode.BadRequest,
                        null);
                };

            mockLevyTransferMatchingService
                .Setup(x => x.AcceptFunding(It.IsAny<AcceptFundingRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(acceptFundingProvider);

            var result = await setApplicationAcceptanceCommandHandler.Handle(setApplicationAcceptanceCommand, CancellationToken.None);

            Assert.IsFalse(result);
        }


        [Test, MoqAutoData]
        public void And_Acceptance_Is_Decline_Throws_NotImplementedException(
            SetApplicationAcceptanceCommand setApplicationAcceptanceCommand,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            SetApplicationAcceptanceCommandHandler setApplicationAcceptanceCommandHandler)
        {
            setApplicationAcceptanceCommand.Acceptance = Types.ApplicationAcceptance.Decline;

            // Act/Assert
            Assert.ThrowsAsync(typeof(NotImplementedException), () => setApplicationAcceptanceCommandHandler.Handle(setApplicationAcceptanceCommand, CancellationToken.None));
        }
    }
}