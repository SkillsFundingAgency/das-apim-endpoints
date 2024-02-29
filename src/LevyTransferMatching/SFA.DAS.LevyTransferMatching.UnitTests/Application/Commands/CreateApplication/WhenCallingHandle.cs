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

            Assert.That(_command.PledgeId, Is.EqualTo(_createApplicationRequest.PledgeId));
            Assert.That(_command.EmployerAccountId, Is.EqualTo(createdApplication.EmployerAccountId));
            Assert.That(_command.Details, Is.EqualTo(createdApplication.Details));
            Assert.That(_command.StandardId, Is.EqualTo(createdApplication.StandardId));
            Assert.That(_getStandardResponse.Title, Is.EqualTo(createdApplication.StandardTitle));
            Assert.That(_getStandardResponse.Level, Is.EqualTo(createdApplication.StandardLevel));
            Assert.That(_getStandardResponse.TypicalDuration, Is.EqualTo(createdApplication.StandardDuration));
            Assert.That(_getStandardResponse.Route, Is.EqualTo(createdApplication.StandardRoute));
            Assert.That(_getStandardResponse.MaxFundingOn(_command.StartDate), Is.EqualTo(createdApplication.StandardMaxFunding));
            Assert.That(_command.NumberOfApprentices, Is.EqualTo(createdApplication.NumberOfApprentices));
            Assert.That(_command.StartDate, Is.EqualTo(createdApplication.StartDate));
            Assert.That(_command.HasTrainingProvider, Is.EqualTo(createdApplication.HasTrainingProvider));
            Assert.That(_command.Sectors, Is.EquivalentTo(createdApplication.Sectors));
            Assert.That(_command.Locations, Is.EqualTo(createdApplication.Locations));
            Assert.That(_command.AdditionalLocation, Is.EqualTo(createdApplication.AdditionalLocation));
            Assert.That(_command.SpecificLocation, Is.EqualTo(createdApplication.SpecificLocation));
            Assert.That(_command.FirstName, Is.EqualTo(createdApplication.FirstName));
            Assert.That(_command.LastName, Is.EqualTo(createdApplication.LastName));
            Assert.That(_command.EmailAddresses, Is.EqualTo(createdApplication.EmailAddresses));
            Assert.That(_command.BusinessWebsite, Is.EquivalentTo(createdApplication.BusinessWebsite));
            Assert.That(_command.UserId, Is.EquivalentTo(createdApplication.UserId));
            Assert.That(_command.UserDisplayName, Is.EquivalentTo(createdApplication.UserDisplayName));
        }

        [Test]
        public async Task Application_Id_Is_Returned()
        {
            var result = await _handler.Handle(_command, CancellationToken.None);
            Assert.That(_response.ApplicationId, Is.EqualTo(result.ApplicationId));
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
