using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetCandidatesByActivity;
using SFA.DAS.Testing.AutoFixture;
using System.Net;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingCandidatesByActivity
    {
        [Test, MoqAutoData]
        public async Task Then_Candidates_Returned_From_Mediator(
            DateTime cutOffDateTime,
            GetCandidateByActivityQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CandidatesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetCandidateByActivityQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

            var actual = await sut.GetCandidatesByActivity(cutOffDateTime, It.IsAny<CancellationToken>()) as ObjectResult;
            var actualValue = actual!.Value as GetCandidateByActivityQueryResult;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
                actual.Value.Should().BeOfType<GetCandidateByActivityQueryResult>();
                actualValue!.Candidates.Should().BeEquivalentTo(mockQueryResult.Candidates);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
            DateTime cutOffDateTime,
            GetCandidateByActivityQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CandidatesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetCandidateByActivityQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

            var actual = await sut.GetCandidatesByActivity(cutOffDateTime, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
