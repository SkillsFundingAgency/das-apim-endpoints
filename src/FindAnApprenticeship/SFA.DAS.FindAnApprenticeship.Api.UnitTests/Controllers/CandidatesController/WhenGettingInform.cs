using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Application.Queries.CreateAccount.Inform;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.CandidatesController
{
    [TestFixture]
    public class WhenGettingInform
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            GetInformQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.CandidatesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetInformQuery>(q =>
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetCreateAccount(candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetInformQueryResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo(queryResult);
        }
    }
}
