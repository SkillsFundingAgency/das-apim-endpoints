using AutoFixture.NUnit3;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetSavedVacancies;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.SavedVacanciesController
{
    [TestFixture]
    public class WhenGettingIndex
    {
        [Test, MoqAutoData]
        public async Task Then_The_Query_Response_Is_Returned(
            Guid candidateId,
            GetSavedVacanciesQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.SavedVacanciesController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetSavedVacanciesQuery>(q =>
                        q.CandidateId == candidateId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(queryResult);

            var actual = await controller.Index(candidateId);

            actual.Should().BeOfType<OkObjectResult>();
            var actualObject = ((OkObjectResult)actual).Value as GetSavedVacanciesQueryResult;
            actualObject.Should().NotBeNull();
            actualObject.Should().Be(queryResult);
        }
    }
}
