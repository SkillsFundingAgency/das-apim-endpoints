using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.Apply.GetApplication;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.ApplicationController;

[TestFixture]
public class WhenGettingSummary
{
    [Test, MoqAutoData]
    public async Task Then_The_Query_Response_Is_Returned(
        Guid candidateId,
        Guid applicationId,
        GetApplicationQueryResult queryResult,
        [Frozen] Mock<IMediator> mediator,
        [Greedy] Api.Controllers.ApplicationController controller)
    {
        mediator.Setup(x => x.Send(It.Is<GetApplicationQuery>(q =>
                    q.CandidateId == candidateId && q.ApplicationId == applicationId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(queryResult);

        var actual = await controller.GetApplicationSummary(applicationId, candidateId);

        actual.Should().BeOfType<OkObjectResult>();
        var actualObject = ((OkObjectResult)actual).Value as GetApplicationApiResponse;
        actualObject.Should().NotBeNull();
        actualObject!.Candidate.Should().BeEquivalentTo(queryResult.CandidateDetails, options => options.Excluding(fil => fil.Address));
        actualObject!.Candidate.Address.Should().BeEquivalentTo(queryResult.CandidateDetails.Address);
        actualObject!.ApplicationQuestions.Should().BeEquivalentTo(queryResult.ApplicationQuestions);
        actualObject!.WorkHistory.Should().BeEquivalentTo(queryResult.WorkHistory);
        actualObject!.DisabilityConfidence.Should().BeEquivalentTo(queryResult.DisabilityConfidence);
        actualObject!.IsDisabilityConfident.Should().Be(queryResult.IsDisabilityConfident); 
        actualObject!.EducationHistory.Should().BeEquivalentTo(queryResult.EducationHistory);
        actualObject!.InterviewAdjustments.Should().BeEquivalentTo(queryResult.InterviewAdjustments);
        actualObject!.ClosedDate.Should().Be(queryResult.ClosedDate);
        actualObject!.ClosingDate.Should().Be(queryResult.ClosingDate);
        actualObject!.VacancyTitle.Should().Be(queryResult.VacancyTitle);
        actualObject!.EmployerName.Should().Be(queryResult.EmployerName);
    }
}