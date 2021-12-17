using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetAmount;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.InnerApi.Responses;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetAmount
{
    [TestFixture]
    public class GetAmountQueryHandlerTests
    {
        private GetAmountQueryHandler _handler;
        private Mock<IAccountsService> _accountsService;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private GetAmountQuery _query;
        private Models.Account _account;
        private GetPledgesResponse _pledges;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetAmountQuery>();
            _account = _fixture.Create<Models.Account>();
            _pledges = _fixture.Create<GetPledgesResponse>();

            _accountsService = new Mock<IAccountsService>();
            _accountsService.Setup(x => x.GetAccount(_query.AccountId))
                .ReturnsAsync(_account);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _levyTransferMatchingService.Setup(x => x.GetPledges(It.IsAny<GetPledgesRequest>()))
                .ReturnsAsync(_pledges);

            _handler = new GetAmountQueryHandler(_accountsService.Object, _levyTransferMatchingService.Object);
        }

        [Test]
        public async Task Handle_Result_Has_Correct_RemainingTransferAllowance()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);

            var currentAmount = 0;

            foreach (var pledge in _pledges.Pledges)
            {
                currentAmount += pledge.Amount;
            }

            Assert.AreEqual(result.RemainingTransferAllowance, _account.RemainingTransferAllowance - currentAmount);
        }

        [Test]
        public async Task Handle_Result_Has_Correct_AccountName()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(result.DasAccountName, _account.DasAccountName);
        }
    }
}
