﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.Application.Demand.Commands.AnonymiseDemand;
using SFA.DAS.EmployerDemand.InnerApi.Requests;
using SFA.DAS.EmployerDemand.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerDemand.UnitTests.Application.Demand.Commands
{
    public class WhenHandlingAnonymiseDemandCommand
    {
        [Test, MoqAutoData]
        public async Task Then_The_Demand_Is_Updated(
            AnonymiseDemandCommand command,
            [Frozen] Mock<IEmployerDemandApiClient<EmployerDemandApiConfiguration>> mockEmployerDemandApiClient,
            AnonymiseDemandCommandHandler handler)
        {
            //Act
            await handler.Handle(command, CancellationToken.None);
            
            //Assert
            mockEmployerDemandApiClient.Verify(x => x.PatchWithResponseCode(
                    It.Is<PatchCourseDemandRequest>(c =>
                        c.PatchUrl.Contains($"api/demand/{command.EmployerDemandId}")
                        && c.Data[0].Path == "ContactEmailAddress"
                        && (string) c.Data[0].Value == string.Empty)),
                Times.Once);
        }
    }
}