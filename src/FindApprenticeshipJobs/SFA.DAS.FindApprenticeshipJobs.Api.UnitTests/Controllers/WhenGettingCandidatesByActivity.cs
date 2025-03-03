using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipJobs.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using System.Net;
using SFA.DAS.FindApprenticeshipJobs.Application.Queries.SavedSearch.GetInactiveCandidates;

namespace SFA.DAS.FindApprenticeshipJobs.Api.UnitTests.Controllers
{
    [TestFixture]
    public class WhenGettingCandidatesByActivity
    {
        [Test, MoqAutoData]
        public async Task Then_Candidates_Returned_From_Mediator(
            int pageNumber,
            int pageSize,
            DateTime cutOffDateTime,
            GetInactiveCandidatesQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CandidatesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetInactiveCandidatesQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(mockQueryResult);

            var actual = await sut.GetInactiveCandidates(cutOffDateTime, pageNumber, pageSize, It.IsAny<CancellationToken>()) as ObjectResult;
            var actualValue = actual!.Value as GetInactiveCandidatesQueryResult;

            using (new AssertionScope())
            {
                actual.StatusCode.Should().Be((int)HttpStatusCode.OK);
                actual.Value.Should().BeOfType<GetInactiveCandidatesQueryResult>();
                actualValue!.Candidates.Should().BeEquivalentTo(mockQueryResult.Candidates);
            }
        }

        [Test, MoqAutoData]
        public async Task And_Exception_Returned_Then_Returns_Internal_Server_Error(
            int pageNumber,
            int pageSize,
            DateTime cutOffDateTime,
            GetInactiveCandidatesQueryResult mockQueryResult,
            [Frozen] Mock<IMediator> mockMediator,
            [Greedy] CandidatesController sut)
        {
            mockMediator.Setup(x => x.Send(It.IsAny<GetInactiveCandidatesQuery>(), It.IsAny<CancellationToken>())).ThrowsAsync(new InvalidOperationException());

            var actual = await sut.GetInactiveCandidates(cutOffDateTime, pageNumber, pageSize, It.IsAny<CancellationToken>()) as StatusCodeResult;

            actual!.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
