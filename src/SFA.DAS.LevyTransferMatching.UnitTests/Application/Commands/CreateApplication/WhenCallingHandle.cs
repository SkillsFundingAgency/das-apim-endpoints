using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private CreateApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<IAccountsService> _accountsService;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountResponse _getAccountResponse;
        private Account _account;
        private CreateApplicationCommand _command;
        private CreateApplicationResponse _response;

        [SetUp]
        public void SetUp()
        {
            _getAccountResponse = _fixture.Create<GetAccountResponse>();
            _command = _fixture.Create<CreateApplicationCommand>();
            _response = _fixture.Create<CreateApplicationResponse>();

            _account = _fixture.Create<Account>();
            _accountsService = new Mock<IAccountsService>();
            _accountsService.Setup(x => x.GetAccount(_command.EncodedAccountId)).ReturnsAsync(_account);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == _command.EmployerAccountId)))
                .ReturnsAsync(_getAccountResponse);

            _levyTransferMatchingService.Setup(x => x.CreateApplication(It.Is<CreateApplicationRequest>(r =>
                r.PledgeId == _command.PledgeId &&
                ((CreateApplicationRequest.CreateApplicationRequestData) r.Data).EmployerAccountId == _command.EmployerAccountId
                ))).ReturnsAsync(_response);

            _handler = new CreateApplicationCommandHandler(_levyTransferMatchingService.Object, _accountsService.Object, Mock.Of<ILogger<CreateApplicationCommandHandler>>());
        }

        [Test]
        public async Task Application_Is_Created_And_Id_Returned()
        {
            var result = await _handler.Handle(_command, CancellationToken.None);
            Assert.AreEqual(_response.ApplicationId, result.ApplicationId);
        }

        [Test]
        public async Task Account_Is_Created_If_It_Not_Already_Exists()
        {
            _levyTransferMatchingService.Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == _command.EmployerAccountId)))
                .ReturnsAsync(() => null);

            await _handler.Handle(_command, CancellationToken.None);

            _levyTransferMatchingService.Verify(x => x.CreateAccount(It.Is<CreateAccountRequest>(r =>
                ((CreateAccountRequest.CreateAccountRequestData)r.Data).AccountId == _command.EmployerAccountId
                && ((CreateAccountRequest.CreateAccountRequestData)r.Data).AccountName == _account.DasAccountName)));
        }
    }
}
