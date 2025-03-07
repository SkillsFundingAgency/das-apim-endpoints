using AutoFixture.NUnit3;
using FluentAssertions;
using Moq;
using SFA.DAS.Earnings.Application.ApprovedApprenticeships;
using SFA.DAS.Earnings.Application.ApprovedApprenticeships.GetApprovedApprenticeships;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Earnings.UnitTests.Application.ApprovedApprenticeships;

public class GetApprovedApprenticeshipsHandlerTests
{
    [Test, MoqAutoData]
        public async Task Handle_Should_Return_Correct_Learner_Data(
            [Frozen] Mock<IApprovedApprenticeshipsStore> datastoreMock,  // Mock of the ILearnerDataStore
            GetApprovedApprenticeshipsQueryHandler handler)
        {
            // Arrange
            const long ukprn = 1000001;
            const int academicYear = 2025;
            const int page = 1;
            const int pageSize = 10;

            var expectedLearnerData = new List<long> { 100, 200, 300 };
            var totalRecords = 30;

            // Mock the datastore methods
            datastoreMock.Setup(ds => ds.Search(ukprn, academicYear, page, pageSize))
                .Returns(expectedLearnerData);  // Return pre-defined list of learner data

            datastoreMock.Setup(ds => ds.Count(ukprn, academicYear))
                .Returns(totalRecords);  // Return total record count

            var query = new GetApprovedApprenticeshipsQuery(ukprn, academicYear, page, pageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRecords.Should().Be((uint)totalRecords);  // Assert TotalRecords matches
            result.Apprenticeships.Should().HaveCount(expectedLearnerData.Count);  // Assert count of learner data

            // Check that each learner data matches expected Uln values
            for (int i = 0; i < expectedLearnerData.Count; i++)
            {
                result.Apprenticeships[i].Uln.Should().Be(expectedLearnerData[i]);
            }

            // Verify that the datastore methods were called
            datastoreMock.Verify(ds => ds.Search(ukprn, academicYear, page, pageSize), Times.Once);
            datastoreMock.Verify(ds => ds.Count(ukprn, academicYear), Times.Once);
        }

        [Test, MoqAutoData]
        public async Task Handle_Should_Return_Empty_Learner_Data_When_No_Results(
            [Frozen] Mock<IApprovedApprenticeshipsStore> datastoreMock, 
            GetApprovedApprenticeshipsQueryHandler handler)
        {
            // Arrange
            const long ukprn = 1000001;
            const int academicYear = 2025;
            const int page = 1;
            const int pageSize = 10;

            var expectedLearnerData = new List<long>();  // No results
            var totalRecords = 0;

            // Mock the datastore methods
            datastoreMock.Setup(ds => ds.Search(ukprn, academicYear, page, pageSize))
                .Returns(expectedLearnerData);  // Return an empty list

            datastoreMock.Setup(ds => ds.Count(ukprn, academicYear))
                .Returns(totalRecords);  // Return 0 records

            var query = new GetApprovedApprenticeshipsQuery(ukprn, academicYear, page, pageSize);

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRecords.Should().Be((uint)totalRecords);  // Assert TotalRecords is 0
            result.Apprenticeships.Should().BeEmpty();  // Assert no learner data returned

            // Verify that the datastore methods were called
            datastoreMock.Verify(ds => ds.Search(ukprn, academicYear, page, pageSize), Times.Once);
            datastoreMock.Verify(ds => ds.Count(ukprn, academicYear), Times.Once);
        }
}