using AutoFixture.NUnit3;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Candidate;
using SFA.DAS.FindAnApprenticeship.Application.Commands.Users;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    public class WhenPuttingUserDetails
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Put_Response(
            string govIdentifier,
            string firstName,
            string lastName,
            string email,
            AddDetailsCommand command,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            var actual = await controller.AddDetails(govIdentifier, firstName, lastName, email);

            mediator.Verify(x => x.Send(It.Is<AddDetailsCommand>(c =>
                c.GovUkIdentifier.Equals(govIdentifier)
                && c.FirstName.Equals(firstName)
                && c.LastName.Equals(lastName)
                && c.Email.Equals(email)
                ), It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
