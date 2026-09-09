using AutoFixture;
using AutoFixture.AutoMoq;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.UnitTests.InnerApi.AodpApi.Qualifications
{
    [TestFixture]
    public class GetChangedQualificationsApiRequestTests
    {
        [Test]
        public void GetUrl_ReturnsBaseStatusQuery_WhenNoOptionalValuesProvided()
        {
            // Arrange
            var request = new GetChangedQualificationsApiRequest();

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=Changed"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithSkipAndTake()
        {
            // Arrange
            var request = new GetChangedQualificationsApiRequest
            {
                Skip = 10,
                Take = 20
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=Changed&Skip=10&Take=20"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithNameOrganisationAndQan()
        {
            // Arrange
            var request = new GetChangedQualificationsApiRequest
            {
                Name = "Test Qualification",
                Organisation = "Test Organisation",
                QAN = "12345678"
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=Changed&Name=Test%20Qualification&Organisation=Test%20Organisation&QAN=12345678"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithMultipleProcessStatusFilters()
        {
            // Arrange
            var processStatus1 = Guid.NewGuid();
            var processStatus2 = Guid.NewGuid();

            var request = new GetChangedQualificationsApiRequest
            {
                ProcessStatusFilter = new List<Guid>
                {
                    processStatus1,
                    processStatus2
                }
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(
                result,
                Is.EqualTo($"api/qualifications?Status=Changed&ProcessStatusFilter={processStatus1}&ProcessStatusFilter={processStatus2}"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithMultipleAgeGroups()
        {
            // Arrange
            var request = new GetChangedQualificationsApiRequest
            {
                AgeGroups = new List<AgeGroup>
                {
                    AgeGroup.Pre16,
                    AgeGroup.SixteenToEighteen
                }
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(
                result,
                Is.EqualTo($"api/qualifications?Status=Changed&AgeGroups={(int)AgeGroup.Pre16}&AgeGroups={(int)AgeGroup.SixteenToEighteen}"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithAllParameters()
        {
            // Arrange
            var processStatus = Guid.NewGuid();

            var request = new GetChangedQualificationsApiRequest
            {
                Skip = 5,
                Take = 15,
                Name = "Qualification",
                Organisation = "Organisation",
                QAN = "QAN123",
                ProcessStatusFilter = new List<Guid> { processStatus },
                AgeGroups = new List<AgeGroup> { AgeGroup.Pre16 }
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(
                result,
                Is.EqualTo(
                    $"api/qualifications?Status=Changed&Skip=5&Take=15&Name=Qualification&Organisation=Organisation&QAN=QAN123&ProcessStatusFilter={processStatus}&AgeGroups={(int)AgeGroup.Pre16}"));
        }
    }
}