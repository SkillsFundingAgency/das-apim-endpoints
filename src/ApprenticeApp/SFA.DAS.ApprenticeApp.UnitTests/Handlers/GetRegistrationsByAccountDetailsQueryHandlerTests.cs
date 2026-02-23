using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Cmad.GetRegistrationsByAccountDetails;
using SFA.DAS.ApprenticeApp.InnerApi.ApprenticeAccounts.Requests;
using SFA.DAS.ApprenticeApp.Models;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetRegistrationsByAccountDetailsQueryHandlerTests
    {
        [Test, MoqAutoData]
        public async Task Handle_Should_Call_ApprenticeCommitmentsApiClient_With_Formatted_Dob_And_Return_Registrations(
            Mock<IApprenticeCommitmentsApiClient<ApprenticeCommitmentsApiConfiguration>> commitmentsApiClientMock,
            GetRegistrationsByAccountDetailsQuery query,
            List<Registration> expectedRegistrations,
            CancellationToken cancellationToken)
        {
            // Arrange - deterministic inputs
            query.FirstName = "Alice";
            query.LastName = "Jones";
            query.DateOfBirth = new System.DateTime(1995, 4, 21);

            var expectedDobString = query.DateOfBirth.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);

            // Setup mock to return the expected registrations when the GetUrl contains the first, last and dob
            commitmentsApiClientMock
                .Setup(c => c.Get<List<Registration>>(
                    It.Is<GetRegistrationsByAccountDetailsRequest>(r =>
                        r.GetUrl.Contains(query.FirstName)
                        && r.GetUrl.Contains(query.LastName)
                        && r.GetUrl.Contains(expectedDobString)
                    )))
                .ReturnsAsync(expectedRegistrations);

            var sut = new GetRegistrationsByAccountDetailsQueryHandler(commitmentsApiClientMock.Object);

            // Act
            var result = await sut.Handle(query, cancellationToken);

            // Assert
            result.Should().NotBeNull();
            result.Registrations.Should().BeSameAs(expectedRegistrations);

            commitmentsApiClientMock.Verify(c => c.Get<List<Registration>>(
                    It.Is<GetRegistrationsByAccountDetailsRequest>(r =>
                        r.GetUrl.Contains(query.FirstName)
                        && r.GetUrl.Contains(query.LastName)
                        && r.GetUrl.Contains(expectedDobString)
                    )),
                Times.Once);
        }
    }
}