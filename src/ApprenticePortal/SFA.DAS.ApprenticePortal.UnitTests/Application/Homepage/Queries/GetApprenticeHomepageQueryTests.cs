using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticePortal.Application.Homepage.Queries;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticePortal.InnerApi.ApprenticeCommitments.Requests;
using SFA.DAS.ApprenticePortal.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SFA.DAS.ApprenticePortal.UnitTests.Application.ApprenticeAccounts.Queries
{
    public class GetApprenticeHomepageQueryTests
    {
        private readonly Fixture _fixture = new Fixture();
        private Apprentice _moqApprentice;
        private List<Apprenticeship> _moqApprenticeships;

        private Guid apprenticeId = Guid.NewGuid();
        private string firstName = "testFirstName", lastName = "testLastName", courseName = "course1", employerName = "employer1";
        
        [Test, MoqAutoData]
        public async Task TestGetApprenticeHomepageQuery(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> accountsApiClient,
            [Frozen] Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> commitmentsApiClient,
            GetApprenticeHomepageQueryHandler sut
            )
        {
            //Arrange
            SetupMoqApprentice();

            accountsApiClient.Setup(client =>
                client.Get<Apprentice>(It.IsAny<GetApprenticeRequest>()))
                .ReturnsAsync(_moqApprentice);

            SetupMoqApprenticeships();

            commitmentsApiClient.Setup(client =>
                client.Get<GetApprenticeApprenticeshipsResult>(It.IsAny<GetApprenticeApprenticeshipsRequest>()))
                .ReturnsAsync(new GetApprenticeApprenticeshipsResult { Apprenticeships = _moqApprenticeships });

            // Act
            var result = await sut.Handle(new GetApprenticeHomepageQuery() { ApprenticeId = apprenticeId }, CancellationToken.None);

            // Assert
            Assert.NotNull(result.apprenticeHomepage);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.ApprenticeId, apprenticeId);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.FirstName, firstName);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.LastName, lastName);
            Assert.AreEqual(result.apprenticeHomepage.Apprenticeship.CourseName, courseName);
            Assert.AreEqual(result.apprenticeHomepage.Apprenticeship.EmployerName, employerName);
        }

        [Test, MoqAutoData]
        public async Task TestGetApprenticeHomepageQueryNoApprenticeships(
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> accountsApiClient,
            [Frozen] Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> commitmentsApiClient,
            GetApprenticeHomepageQueryHandler sut
            )
        {
            //Arrange
            SetupMoqApprentice();

            accountsApiClient.Setup(client =>
                client.Get<Apprentice>(It.IsAny<GetApprenticeRequest>()))
                .ReturnsAsync(_moqApprentice);

            SetupBlankMoqApprenticeships();

            commitmentsApiClient.Setup(client =>
                client.Get<GetApprenticeApprenticeshipsResult>(It.IsAny<GetApprenticeApprenticeshipsRequest>()))
                .ReturnsAsync(new GetApprenticeApprenticeshipsResult { Apprenticeships = _moqApprenticeships });

            // Act
            var result = await sut.Handle(new GetApprenticeHomepageQuery() { ApprenticeId = apprenticeId }, CancellationToken.None);

            // Assert
            Assert.NotNull(result.apprenticeHomepage);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.ApprenticeId, apprenticeId);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.FirstName, firstName);
            Assert.AreEqual(result.apprenticeHomepage.Apprentice.LastName, lastName);
            Assert.IsNull(result.apprenticeHomepage.Apprenticeship);            
        }

        private void SetupMoqApprentice()
        {
            _moqApprentice = _fixture.Build<Apprentice>()
                .With(p => p.ApprenticeId, apprenticeId)
                .With(p => p.FirstName, firstName)
                .With(p => p.LastName, lastName)
                .Create();
        }

        private void SetupMoqApprenticeships()
        {
            _moqApprenticeships = new List<Apprenticeship>() {
                _fixture.Build<Apprenticeship>()
                    .With(p => p.ApprenticeId, apprenticeId)
                    .With(p => p.CourseName, courseName)
                    .With(p => p.EmployerName, employerName)
                    .With(p => p.ConfirmedOn, DateTime.Now)
                    .Create()};
        }

        private void SetupBlankMoqApprenticeships()
        {
            _moqApprenticeships = new List<Apprenticeship>();                
        }
    }
}
