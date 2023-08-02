using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Queries.Pledges.GetOrganisationName;
using SFA.DAS.LevyTransferMatching.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Queries.Pledges.GetOrganisationName
{
    public  class GetOrganisationNameHandlerTests
    {
        private GetOrganisationNameQueryHandler _handler;
        private Mock<IAccountsService> _accountsService;
        private GetOrganisationNameQuery _query;
        private Models.Account _account;
        private readonly Fixture _fixture = new Fixture();

        [SetUp]
        public void SetUp()
        {
            _query = _fixture.Create<GetOrganisationNameQuery>();
            _account = _fixture.Create<Models.Account>();

            _accountsService = new Mock<IAccountsService>();
            _accountsService.Setup(x => x.GetAccount(_query.EncodedAccountId))
                .ReturnsAsync(_account);

            _handler = new GetOrganisationNameQueryHandler(_accountsService.Object);
        }

        [Test]
        public async Task Handle_Result_Has_Correct_DasAccountName()
        {
            var result = await _handler.Handle(_query, CancellationToken.None);
            Assert.AreEqual(result.DasAccountName, _account.DasAccountName);
        }       
    }
}
