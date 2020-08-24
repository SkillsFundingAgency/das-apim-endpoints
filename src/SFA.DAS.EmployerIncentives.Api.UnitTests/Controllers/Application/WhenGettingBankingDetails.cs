using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerIncentives.Api.Controllers;
using SFA.DAS.EmployerIncentives.Api.Models;
using SFA.DAS.EmployerIncentives.Application.Queries.GetBankingData;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.EmployerIncentives.Api.UnitTests.Controllers.EligibleApprenticeshipSearch
{
    public class WhenGettingBankingDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Gets_Banking_Details_From_Mediator(
            long accountId,
            Guid applicationId,
            string hashedAccountId,
            GetBankingDataResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy]ApplicationController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<GetBankingDataQuery>(c=>
                                            c.AccountId.Equals(accountId) 
                                            && c.ApplicationId.Equals(applicationId)
                                            && c.HashedAccountId.Equals(hashedAccountId)),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.GetBankingDetails(accountId, applicationId, hashedAccountId) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as BankingDetailsDto;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo(mediatorResult.Data);
        }
    }
}