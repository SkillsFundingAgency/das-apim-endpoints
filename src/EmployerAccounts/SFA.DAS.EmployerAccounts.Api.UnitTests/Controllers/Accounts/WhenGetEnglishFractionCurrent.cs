using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.EmployerAccounts.Application.Queries.GetEnglishFractionCurrent;
using SFA.DAS.EmployerFinance.Api.Controllers;

namespace SFA.DAS.EmployerAccounts.Api.UnitTests.Controllers.Accounts
{
    public class WhenGetEnglishFractionCurrent
    {
        private Mock<IMediator> _mediator;
        private Mock<ILogger<AccountsController>> _logger;

        private AccountsController _sut;

        [SetUp]
        public void Arrange()
        {
            _mediator = new Mock<IMediator>();
            _logger = new Mock<ILogger<AccountsController>>();

            _sut = new AccountsController(_mediator.Object, _logger.Object);
        }

        [Test]
        public async Task Then_The_Query_Is_Sent_With_CorrectParameters()
        {
            //Arrange
            var hashedAccountId = "6D8LY9";
            var empRefs = new string[] { "333/4444L", "222/3333L" };

            //Act
            await _sut.GetEnglishFractionCurrent(hashedAccountId, empRefs);

            //Assert
            _mediator.Verify(v => v.Send(
                It.Is<GetEnglishFractionCurrentQuery>(p => p.HashedAccountId == hashedAccountId && p.EmpRefs == empRefs), 
                It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
