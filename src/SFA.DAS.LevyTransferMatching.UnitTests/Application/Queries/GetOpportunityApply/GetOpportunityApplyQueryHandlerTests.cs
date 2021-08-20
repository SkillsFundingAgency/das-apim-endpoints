using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Opportunity.GetOpportunityApply;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetUserAccounts
{
    [TestFixture]
    public class GetOpportunityApplyQueryHandlerTests
    {
        private Fixture _fixture;
        private Mock<IUserService> _userService;
        private GetOpportunityApplyQueryHandler _getOpportunityApplyQueryHandler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _userService = new Mock<IUserService>();
            _getOpportunityApplyQueryHandler = new GetOpportunityApplyQueryHandler(_userService.Object);
        }

        [Test]
        public async Task Handle_Returns_UserAccounts()
        {
            string userId = _fixture.Create<string>();
            IEnumerable<UserAccount> userAccounts = _fixture.CreateMany<UserAccount>();

            _userService
                .Setup(x => x.GetUserAccounts(It.Is<string>(y => y == userId)))
                .ReturnsAsync(userAccounts);

            GetOpportunityApplyQuery getOpportunityApplyQuery = new GetOpportunityApplyQuery()
            {
                UserId = userId,
            };

            var result = await _getOpportunityApplyQueryHandler.Handle(getOpportunityApplyQuery, CancellationToken.None);

            Assert.AreEqual(userAccounts.Count(), result.Accounts.Count());
        }
    }
}