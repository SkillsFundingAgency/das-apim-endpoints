using AutoFixture;
using AutoFixture.NUnit3;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.ClosePledge;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.ClosePledge
{
    [TestFixture]
    public class WhenCallingHandle
    {
        [Test, MoqAutoData]
        public async Task Then_Set_Pledge_Status_to_Closed_By_PledgeId(
            ClosePledgeCommand closePledgeCommand,
            int pledgeId,
            ClosePledgeResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            ClosePledgeCommandHandler createPledgeHandler)
        {
            mockLevyTransferMatchingService
                .Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
                .ReturnsAsync(response);

            closePledgeCommand.PledgeId = pledgeId;

            var result = await createPledgeHandler.Handle(closePledgeCommand, CancellationToken.None);

            Assert.IsNotNull(result);
            Assert.IsTrue(result.Updated);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Response_Is_Null_Then_No_Change_To_Pledge(
            ClosePledgeCommand closePledgeCommand,
            int pledgeId,
            ClosePledgeResponse response,
            [Frozen] Mock<ILevyTransferMatchingService> mockLevyTransferMatchingService,
            ClosePledgeCommandHandler createPledgeHandler)
        {
            response = null;
            mockLevyTransferMatchingService
                .Setup(x => x.ClosePledge(It.IsAny<ClosePledgeRequest>()))
                .ReturnsAsync(response);

            closePledgeCommand.PledgeId = pledgeId;

            var result = await createPledgeHandler.Handle(closePledgeCommand, CancellationToken.None);

            Assert.IsFalse(result.Updated);
        }
    }
}