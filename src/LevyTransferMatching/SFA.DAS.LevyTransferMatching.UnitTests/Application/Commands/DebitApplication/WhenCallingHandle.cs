using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.LevyTransferMatching.Application.Commands.DebitApplication;
using SFA.DAS.LevyTransferMatching.InnerApi.LevyTransferMatching.Requests;
using SFA.DAS.LevyTransferMatching.InnerApi.Requests.Applications;
using SFA.DAS.LevyTransferMatching.InnerApi.Responses;
using SFA.DAS.LevyTransferMatching.Interfaces;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.InnerApi.Requests;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.LevyTransferMatching.UnitTests.Application.Commands.DebitApplication
{
    [TestFixture]
    public class WhenCallingHandle
    {
        private DebitApplicationCommandHandler _handler;
        private Mock<ILevyTransferMatchingService> _levyTransferMatchingService;
        private Mock<ICoursesApiClient<CoursesApiConfiguration>> _coursesApiClient;
        private readonly Fixture _fixture = new Fixture();

        private GetApplicationResponse _getApplicationResponse;

        private GetStandardsListItem _coursesApiResponse;

        private DebitApplicationCommand _debitApplicationCommand;
        private DebitApplicationRequest _debitApplicationRequest;

        [SetUp]
        public void Setup()
        {
            _debitApplicationCommand = _fixture.Create<DebitApplicationCommand>();
            _getApplicationResponse = _fixture.Create<GetApplicationResponse>();
            _coursesApiResponse = _fixture.Create<GetStandardsListItem>();

            var applicationApiResponse = new ApiResponse<DebitApplicationRequest>(new DebitApplicationRequest(_debitApplicationCommand.ApplicationId, new DebitApplicationRequest.DebitApplicationRequestData()), HttpStatusCode.OK, string.Empty);

            _levyTransferMatchingService = new Mock<ILevyTransferMatchingService>();
            _coursesApiClient = new Mock<ICoursesApiClient<CoursesApiConfiguration>>();

            _levyTransferMatchingService.Setup(x => x.GetApplication(It.Is<GetApplicationRequest>(y => y.GetUrl.Contains(_debitApplicationCommand.ApplicationId.ToString()))))
                .ReturnsAsync(_getApplicationResponse);

            _levyTransferMatchingService.Setup(x => x.DebitApplication(It.IsAny<DebitApplicationRequest>()))
                .Callback<DebitApplicationRequest>(r => _debitApplicationRequest = r)
                .ReturnsAsync(applicationApiResponse);

            _coursesApiClient.Setup(x => x.Get<GetStandardsListItem>(It.Is<GetStandardDetailsByIdRequest>(y => y.GetUrl.Contains(_getApplicationResponse.StandardId))))
                .ReturnsAsync(_coursesApiResponse);

            _handler = new DebitApplicationCommandHandler(_levyTransferMatchingService.Object, _coursesApiClient.Object);
        }

        [Test]
        public async Task Application_Is_Debited()
        {
            await _handler.Handle(_debitApplicationCommand, CancellationToken.None);

            var debit = (DebitApplicationRequest.DebitApplicationRequestData)_debitApplicationRequest.Data;

            Assert.That(_debitApplicationRequest.PostUrl, Is.EqualTo($"applications/{_debitApplicationCommand.ApplicationId}/debit"));
            Assert.That(_debitApplicationCommand.Amount, Is.EqualTo(debit.Amount));
            Assert.That(_debitApplicationCommand.NumberOfApprentices, Is.EqualTo(debit.NumberOfApprentices));
        }
    }
}
