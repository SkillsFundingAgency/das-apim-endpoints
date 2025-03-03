using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models.Applications;
using SFA.DAS.FindAnApprenticeship.Application.Queries.GetEqualityQuestions;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Controllers.EqualityQuestionsController
{
    [TestFixture]
    public class WhenGettingEqualityQuestions
    {
        [Test, MoqAutoData]
        public async Task Then_Returns_Ok_Result(
            Guid candidateId,
            GetEqualityQuestionsQueryResult queryResult,
            [Frozen] Mock<IMediator> mediator,
            [Greedy] Api.Controllers.EqualityQuestionsController controller)
        {
            mediator.Setup(x => x.Send(It.Is<GetEqualityQuestionsQuery>(x => x.CandidateId == candidateId), CancellationToken.None))
                .ReturnsAsync(queryResult);

            var actual = await controller.Get(candidateId);

            var okResult = actual.Should().BeOfType<OkObjectResult>().Subject;
            var apiResponse = okResult.Value.Should().BeOfType<GetEqualityQuestionsApiResponse>().Subject;
            apiResponse.Should().BeEquivalentTo(queryResult);
        }
    }
}
