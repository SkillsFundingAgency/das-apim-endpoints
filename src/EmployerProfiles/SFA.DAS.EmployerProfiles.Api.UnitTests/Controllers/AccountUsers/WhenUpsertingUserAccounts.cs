using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.EmployerProfiles.Api.Controllers;
using SFA.DAS.EmployerProfiles.Api.Models;
using SFA.DAS.EmployerProfiles.Application.AccountUsers.Commands.UpsertEmployer;

namespace SFA.DAS.EmployerProfiles.Api.UnitTests.Controllers.AccountUsers
{
    public class WhenUpsertingUserAccounts
    {
        [Test, MoqAutoData]
        public async Task Then_Upsert_UserAccount_From_Mediator(
            Guid id,
            string email,
            string firstName,
            string lastName,
            string govIdentifier,
            UpsertAccountCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpsertAccountCommand>(c => c.Email.Equals(email)
                                                     && c.FirstName.Equals(firstName)
                                                     && c.LastName.Equals(lastName)
                                                     && c.GovIdentifier.Equals(govIdentifier)
                                                     && c.Id.Equals(id)
                                                     ),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mediatorResult);

            var controllerResult = await controller.UpsertUserAccount(id, new UpsertAccountRequest
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                GovIdentifier = govIdentifier
            }) as ObjectResult;

            Assert.IsNotNull(controllerResult);
            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.OK);
            var model = controllerResult.Value as UpsertAccountCommandResult;
            Assert.IsNotNull(model);
            model.Should().BeEquivalentTo((UpsertAccountCommandResult)mediatorResult);
        }

        [Test, MoqAutoData]
        public async Task UpsertUserAccount_And_Exception_Then_Returns_Internal_Server_Error(
            Guid id,
            string email,
            string firstName,
            string lastName,
            string govIdentifier,
            UpsertAccountCommandResult mediatorResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] AccountUsersController controller)
        {
            mockMediator
                .Setup(mediator => mediator.Send(
                    It.Is<UpsertAccountCommand>(c => c.Email.Equals(email)
                                                     && c.FirstName.Equals(firstName)
                                                     && c.LastName.Equals(lastName)
                                                     && c.GovIdentifier.Equals(govIdentifier)
                                                     && c.Id.Equals(id)
                    ),
                    It.IsAny<CancellationToken>()))
                .ThrowsAsync(new InvalidOperationException());

            var controllerResult = await controller.UpsertUserAccount(id, new UpsertAccountRequest
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName,
                GovIdentifier = govIdentifier
            }) as StatusCodeResult;

            controllerResult.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}