using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.SignIntoYourOldAccount;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.CandidatesController
{
    [TestFixture]
    public class WhenGettingSignIntoYourOldAccount
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            string email,
            string password,
            GetSignIntoYourOldAccountQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.CandidatesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetSignIntoYourOldAccountQuery>(q =>
                        q.CandidateId == candidateId
                        && q.Email == email
                        && q.Password == password),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.SignIntoYourOldAccount(candidateId, email, password);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetSignIntoYourOldAccountQueryResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(queryResult);
        }
    }
}
