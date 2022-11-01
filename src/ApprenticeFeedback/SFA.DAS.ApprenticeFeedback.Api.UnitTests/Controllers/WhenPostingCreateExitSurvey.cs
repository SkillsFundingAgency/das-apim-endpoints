using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeFeedback.Api.Controllers;
using SFA.DAS.ApprenticeFeedback.Application.Commands.CreateExitSurvey;
using SFA.DAS.Testing.AutoFixture;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticeFeedback.Api.UnitTests.Controllers
{
    public class WhenPostingCreateExitSurvey
    {
        private Mock<IMediator> _mockMediator;
        private ExitSurveyController _controller;

        [SetUp]
        public void Arrange()
        {
            _mockMediator = new Mock<IMediator>();

            _controller = new ExitSurveyController(_mockMediator.Object, Mock.Of<ILogger<ExitSurveyController>>());
        }

        [Test, MoqAutoData]
        public async Task And_CommandIsProcessedSuccessfully_Then_ReturnCreated(CreateExitSurveyCommand request)
        {
            var result = await _controller.CreateExitSurvey(request);

            result.Should().NotBeNull();
        }
    }
}
