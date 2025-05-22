using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Recruit.Api.Controllers;
using SFA.DAS.Recruit.Api.Models;
using SFA.DAS.Recruit.Application.Candidates.Queries.GetApplicationsById;
using System;
using System.Net;
using System.Threading;

namespace SFA.DAS.Recruit.Api.UnitTests.Controllers.Applications
{
    [TestFixture]
    public class WhenCallingGetAllApplications
    {
        [Test, MoqAutoData]
        public async Task Then_The_Mediator_Query_Is_Sent_And_The_Candidate_Data_Returned(
            GetAllApplicationsByIdApiRequest request,
            GetApplicationsByIdQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplicationsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetApplicationsByIdQuery>(c => c.ApplicationIds == request.ApplicationIds && c.IncludeDetails == request.IncludeDetails), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.GetApplications(request) as OkObjectResult;

            actual.Should().NotBeNull();
            var actualModel = actual!.Value as GetApplicationsByIdQueryResult;
            actualModel.Should().BeEquivalentTo(queryResult);
        }

        [Test, MoqAutoData]
        public async Task Then_If_Exception_Thrown_From_Mediator_Query_Then_InternalServerError_Response_Returned(
            GetAllApplicationsByIdApiRequest request,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] ApplicationsController controller)
        {

            mediator.Setup(x => x.Send(It.Is<GetApplicationsByIdQuery>(c => c.ApplicationIds == request.ApplicationIds && c.IncludeDetails == request.IncludeDetails), CancellationToken.None))
                .ThrowsAsync(new Exception());

            var actual = await controller.GetApplications(request) as StatusCodeResult;

            actual.Should().NotBeNull();
            actual.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}