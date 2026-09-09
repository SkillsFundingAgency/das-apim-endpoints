using AutoFixture;
using AutoFixture.AutoMoq;
using SFA.DAS.Aodp.InnerApi.AodpApi.Qualifications;
using SFA.DAS.Aodp.Models;

namespace SFA.DAS.Aodp.UnitTests.InnerApi.AodpApi.Qualifications
{
    [TestFixture]
    public class GetNewQualificationsApiRequestTests
    {
        [Test]
        public void GetUrl_ReturnsBaseStatusQuery_WhenNoOptionalValuesProvided()
        {
            // Arrange
            var request = new GetNewQualificationsApiRequest();

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=New"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithSkipAndTake()
        {
            // Arrange
            var request = new GetNewQualificationsApiRequest
            {
                Skip = 10,
                Take = 20
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=New&Skip=10&Take=20"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithNameOrganisationAndQan()
        {
            // Arrange
            var request = new GetNewQualificationsApiRequest
            {
                Name = "Test Qualification",
                Organisation = "Test Organisation",
                QAN = "12345678"
            };

            // Act
            var result = request.GetUrl;

            // Assert
            Assert.That(result, Is.EqualTo("api/qualifications?Status=New&Name=Test%20Qualification&Organisation=Test%20Organisation&QAN=12345678"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithMultipleProcessStatusFilters()
        {
            // Arrange
            var processStatus1 = Guid.NewGuid();
            var processStatus2 = Guid.NewGuid();

            var request = new GetNewQualificationsApiRequest
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
                Is.EqualTo($"api/qualifications?Status=New&ProcessStatusFilter={processStatus1}&ProcessStatusFilter={processStatus2}"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithMultipleAgeGroups()
        {
            // Arrange
            var request = new GetNewQualificationsApiRequest
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
                Is.EqualTo($"api/qualifications?Status=New&AgeGroups={(int)AgeGroup.Pre16}&AgeGroups={(int)AgeGroup.SixteenToEighteen}"));
        }

        [Test]
        public void GetUrl_ReturnsUrlWithAllParameters()
        {
            // Arrange
            var processStatus = Guid.NewGuid();

            var request = new GetNewQualificationsApiRequest
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
                    $"api/qualifications?Status=New&Skip=5&Take=15&Name=Qualification&Organisation=Organisation&QAN=QAN123&ProcessStatusFilter={processStatus}&AgeGroups={(int)AgeGroup.Pre16}"));
        }
    }
}