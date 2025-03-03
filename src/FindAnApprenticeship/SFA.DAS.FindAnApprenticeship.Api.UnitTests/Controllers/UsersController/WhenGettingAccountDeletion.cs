using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Users.GetAccountDeletionQuery;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.UsersController
{
    [TestFixture]
    public class WhenGettingAccountDeletion
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            GetAccountDeletionQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.UsersController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetAccountDeletionQuery>(q =>
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.AccountDeletion(candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetAccountDeletionQueryResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().Be(queryResult);
        }
    }
}