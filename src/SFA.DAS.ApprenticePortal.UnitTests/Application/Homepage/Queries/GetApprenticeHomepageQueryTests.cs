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

        [Test, MoqAutoData]
        public async Task TestGetApprenticeHomepageQuery(            
            [Frozen] Mock<IApprenticeAccountsApiClient<ApprenticeAccountsApiConfiguration>> accountsApiClient,
            [Frozen] Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> commitmentsApiClient,
            GetApprenticeHomepageQueryHandler sut
            )
        {
            //Arrange
            var apprenticeId = new Guid("80e8b73c-5c3a-11ec-bf63-0242ac130002");
            string firstName = "testFirstName", lastName = "testLastName", courseName = "course1", employerName = "employer1";

            var moqApprentice = _fixture.Build<Apprentice>()
                .With(p => p.ApprenticeId, apprenticeId)
                .With(p => p.FirstName, firstName)
                .With(p => p.LastName, lastName)
                .Create();

            accountsApiClient.Setup(client =>
                client.Get<Apprentice>(It.IsAny<GetApprenticeRequest>()))
                .ReturnsAsync(moqApprentice);

            var moqApprenticeships = new List<Apprenticeship>() { 
                _fixture.Build<Apprenticeship>()
                    .With(p => p.ApprenticeId, apprenticeId)
                    .With(p => p.CourseName, courseName)
                    .With(p => p.EmployerName, employerName)
                    .With(p => p.ConfirmedOn, DateTime.Now)
                    .Create()};

            commitmentsApiClient.Setup(client =>
                client.Get<GetApprenticeApprenticeshipsResult>(It.IsAny<GetApprenticeApprenticeshipsRequest>()))
                .ReturnsAsync(new GetApprenticeApprenticeshipsResult { apprenticeships = moqApprenticeships });
                            
            // Act
            var result = await sut.Handle(new GetApprenticeHomepageQuery() { ApprenticeId = apprenticeId }, CancellationToken.None);

            // Assert
            Assert.NotNull(result.apprenticeHomepage);
            Assert.AreEqual(result.apprenticeHomepage.ApprenticeId, apprenticeId);
            Assert.AreEqual(result.apprenticeHomepage.FirstName, firstName);
            Assert.AreEqual(result.apprenticeHomepage.LastName, lastName);
            Assert.AreEqual(result.apprenticeHomepage.CourseName, courseName);
            Assert.AreEqual(result.apprenticeHomepage.EmployerName, employerName);
            Assert.IsTrue(result.apprenticeHomepage.ApprenticeshipComplete);
        }
    }
}
