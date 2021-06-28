using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.GetUserAccounts;
using SFA.DAS.LevyTransferMatching.Application.Services;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.GetUserAccounts
{
    [TestFixture]
    public class GetUserAccountsHandlerTests
    {
        private Fixture _fixture;
        private Mock<IUserService> _userService;
        private GetUserAccountsHandler _getUserAccountsHandler;

        [SetUp]
        public void Setup()
        {
            _fixture = new Fixture();
            _userService = new Mock<IUserService>();
            _getUserAccountsHandler = new GetUserAccountsHandler(_userService.Object);
        }

        [Test]
        public async Task Handle_Returns_Accounts()
        {
            string userId = _fixture.Create<string>();
            IEnumerable<Account> accounts = _fixture.CreateMany<Account>();

            _userService
                .Setup(x => x.GetUserAccounts(It.Is<string>(y => y == userId)))
                .ReturnsAsync(accounts);

            GetUserAccountsQuery getUserAccountsQuery = new GetUserAccountsQuery()
            {
                UserId = userId,
            };

            var result = await _getUserAccountsHandler.Handle(getUserAccountsQuery, CancellationToken.None);

            CollectionAssert.AreEqual(accounts, result.Accounts);
        }
    }
}