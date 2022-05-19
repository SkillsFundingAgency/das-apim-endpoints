using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.CreateApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Responses;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.LevyTransferMatching.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.CreateApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private CreateApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<IAccountsService> _accountsService;
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
        private readonly Fixture _fixture = new Fixture();
        private GetAccountResponse _getAccountResponse;
        private GetStandardsListItem _getStandardResponse;
        private Account _account;
        private CreateApplicationCommand _command;
        private CreateApplicationResponse _response;
        private CreateApplicationRequest _createApplicationRequest;

        [SetUp]
        public void SetUp()
        {
            _getAccountResponse = _fixture.Create<GetAccountResponse>();
            _command = _fixture.Create<CreateApplicationCommand>();
            _response = _fixture.Create<CreateApplicationResponse>();
            _getStandardResponse = _fixture.Create<GetStandardsListItem>();

            _account = _fixture.Create<Account>();
            _accountsService = new Mock<IAccountsService>();
            _accountsService.Setup(x => x.GetAccount(_command.EncodedAccountId)).ReturnsAsync(_account);

            _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();
            _coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(r => r.Id == _command.StandardId)))
                .ReturnsAsync(_getStandardResponse);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();

            _levyTransferMatchingService.Setup(x => x.GetAccount(It.Is<GetAccountRequest>(r => r.AccountId == _command.EmployerAccountId)))
                .ReturnsAsync(_getAccountResponse);

            _levyTransferMatchingService.Setup(x => x.CreateApplication(It.IsAny<CreateApplicationRequest>()))
                .Callback<CreateApplicationRequest>(r => _createApplicationRequest = r)
                .ReturnsAsync(_response);

            _handler = new CreateApplicationCommandHandler(_levyTransferMatchingService.Object, _accountsService.Object, Mock.Of<ILogger<CreateApplicationCommandHandler>>(), _coursesApiClient.Object);
        }

        [Test]
        public async Task Application_Is_Created()
        {
            await _handler.Handle(_command, CancellationToken.None);

            var createdApplication = (CreateApplicationRequestData) _createApplicationRequest.Data;

            Assert.AreEqual(_command.PledgeId, _createApplicationRequest.PledgeId);
            Assert.AreEqual(_command.EmployerAccountId, createdApplication.EmployerAccountId);
            Assert.AreEqual(_command.Details, createdApplication.Details);
            Assert.AreEqual(_command.StandardId, createdApplication.StandardId);
            Assert.AreEqual(_getStandardResponse.Title, createdApplication.StandardTitle);
            Assert.AreEqual(_getStandardResponse.Level, createdApplication.StandardLevel);
            Assert.AreEqual(_getStandardResponse.TypicalDuration, createdApplication.StandardDuration);
            Assert.AreEqual(_getStandardResponse.Route, createdApplication.StandardRoute);
            Assert.AreEqual(_getStandardResponse.MaxFundingOn(_command.StartDate), createdApplication.StandardMaxFunding);
            Assert.AreEqual(_command.NumberOfApprentices, createdApplication.NumberOfApprentices);
            Assert.AreEqual(_command.StartDate, createdApplication.StartDate);
            Assert.AreEqual(_command.HasTrainingProvider, createdApplication.HasTrainingProvider);
            CollectionAssert.AreEqual(_command.Sectors, createdApplication.Sectors);
            CollectionAssert.AreEqual(_command.Locations, createdApplication.Locations);
            Assert.AreEqual(_command.AdditionalLocation, createdApplication.AdditionalLocation);
            Assert.AreEqual(_command.SpecificLocation, createdApplication.SpecificLocation);
            Assert.AreEqual(_command.FirstName, createdApplication.FirstName);
            Assert.AreEqual(_command.LastName, createdApplication.LastName);
            CollectionAssert.AreEqual(_command.EmailAddresses, createdApplication.EmailAddresses);
            Assert.AreEqual(_command.BusinessWebsite, createdApplication.BusinessWebsite);
            Assert.AreEqual(_command.UserId, createdApplication.UserId);
            Assert.AreEqual(_command.UserDisplayName, createdApplication.UserDisplayName);
        }

        [Test]
        public async Task Application_Id_Is_Returned()
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
