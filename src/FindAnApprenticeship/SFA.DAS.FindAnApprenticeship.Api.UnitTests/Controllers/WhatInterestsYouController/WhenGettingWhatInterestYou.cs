using System.Threading;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.WhatInterestsYou;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.WhatInterestsYouController
{
    [TestFixture]
    public class WhenGettingWhatInterestYou
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            Guid applicationId,
            GetWhatInterestsYouQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.WhatInterestsYouController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetWhatInterestsYouQuery>(q =>
                        q.CandidateId == candidateId && q.ApplicationId == applicationId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetWhatInterestsYou(applicationId, candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetWhatInterestsYouApiResponse;
            actualObject.Should().NotBeNull();
            actualObject.Should().BeEquivalentTo((GetWhatInterestsYouApiResponse)queryResult);
        }
    }
}
