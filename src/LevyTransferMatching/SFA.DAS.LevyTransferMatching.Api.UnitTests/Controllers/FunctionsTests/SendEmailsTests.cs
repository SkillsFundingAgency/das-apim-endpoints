using AutoFixture;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Api.Controllers;
using SFA.DAS.LevyTransferMatching.Api.Models.Functions;
using SFA.DAS.LevyTransferMatching.Application.Commands.SendEmails;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.Api.UnitTests.Controllers.FunctionsTests
{
    [TestFixture]
    public class SendEmailsTests
    {
        private FunctionsController _controller;
        private Mock<IMediator> _mediator;
        private SendEmailsRequest _request;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void Setup()
        {
            _mediator = new Mock<IMediator>();

            _request = _fixture.Create<SendEmailsRequest>();

            _controller = new FunctionsController(_mediator.Object, Mock.Of<ILogger<FunctionsController>>());
        }

        [Test]
        public async Task Send_Emails_Calls_Handler()
        {
            var result = await _controller.SendEmails(_request);

            _mediator.Verify(x =>
                x.Send(It.Is<SendEmailsCommand>(c => c.EmailDataList.Count == _request.EmailDataList.Count && c.EmailDataList[0].RecipientEmailAddress == _request.EmailDataList[0].RecipientEmailAddress),
                    It.IsAny<CancellationToken>()));
        }
    }
}
