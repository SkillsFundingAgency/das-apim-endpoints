using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetAmount
{
    [TestFixture]
    public class GetAmountQueryHandlerTests
    {
        private GetAmountQueryHandler _handler;
        private Mock<IAccountsService> _accountsService;
        private GetAmountQuery _query;
        private Models.Account _account;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetAmountQuery>();
            _account = _fixture.Create<Models.Account>();

            _accountsService = new Mock<IAccountsService>();
            _accountsService.Setup(x => x.GetAccount(_query.EncodedAccountId))
                .ReturnsAsync(_account);

            _handler = new GetAmountQueryHandler(_accountsService.Object);
        }

        [Test]
        public async Task Handle_Result_Has_Correct_RemainingTransferAllowance()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(result.RemainingTransferAllowance, _account.RemainingTransferAllowance);
        }

      
    }
}
