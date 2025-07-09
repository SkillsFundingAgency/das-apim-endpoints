using SFA.DAS.FindAnApprenticeship.Services;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Services.VacancyService
{
    [TestFixture]
    public class WhenGettingWorkLocation
    {
        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_AcrossEngland_ReturnsRecruitingNationally(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            vacancy.Setup(v => v.EmployerLocationOption).Returns(AvailableWhere.AcrossEngland);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object);

            // Assert
            result.Should().BeEquivalentTo("Recruiting nationally");
        }

        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_MultipleLocations_ReturnsEmploymentLocations(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var address = new Address { AddressLine1 = "Line1", Postcode = "Postcode" };
            var otherAddresses = new List<Address> { new Address { AddressLine1 = "OtherLine1", Postcode = "OtherPostcode" } };
            vacancy.Setup(v => v.EmployerLocationOption).Returns(AvailableWhere.MultipleLocations);
            vacancy.Setup(v => v.Address).Returns(address);
            vacancy.Setup(v => v.OtherAddresses).Returns(otherAddresses);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object);

            // Assert
            result.Should().Be("Line1 (Postcode) and 1 other available locations");
        }

        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_MultipleLocations_ReturnsEmploymentLocationsCityNames(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var address = new Address { AddressLine1 = "Line1", AddressLine4 = "City1" ,Postcode = "Postcode" };
            var otherAddresses = new List<Address> { new Address { AddressLine1 = "OtherLine1", AddressLine4 = "City2", Postcode = "OtherPostcode" } };
            vacancy.Setup(v => v.EmployerLocationOption).Returns(AvailableWhere.MultipleLocations);
            vacancy.Setup(v => v.Address).Returns(address);
            vacancy.Setup(v => v.OtherAddresses).Returns(otherAddresses);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object, true);

            // Assert
            result.Should().Be("City1, City2");
        }

        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_Anon_MultipleLocations_ReturnsEmploymentLocations(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var address = new Address { AddressLine3 = "Leeds", Postcode = "LS6" };
            var otherAddresses = new List<Address>
            {
                new Address {AddressLine3 = "Leeds", Postcode = "LS16"},
                new Address {AddressLine3 = "Leeds", Postcode = "LS9"}
            };
            vacancy.Setup(v => v.EmployerLocationOption).Returns(AvailableWhere.MultipleLocations);
            vacancy.Setup(v => v.Address).Returns(address);
            vacancy.Setup(v => v.OtherAddresses).Returns(otherAddresses);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object);

            // Assert
            result.Should().Be("Leeds (LS6) and 2 other available locations");
        }

        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_OneLocation_ReturnsOneLocationCityName(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var address = new Address { AddressLine1 = "Line1", Postcode = "Postcode" };
            vacancy.Setup(v => v.EmployerLocationOption).Returns(AvailableWhere.OneLocation);
            vacancy.Setup(v => v.Address).Returns(address);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object);

            // Assert
            result.Should().Be("Line1 (Postcode)");
        }

        [Test, MoqAutoData]
        public void GetVacancyWorkLocation_NullLocationOption_ReturnsOneLocationCityName(
            Mock<IVacancy> vacancy,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            [Frozen] Mock<IRecruitApiClient<RecruitApiConfiguration>> recruitApiClient,
            FindAnApprenticeship.Services.VacancyService service)
        {
            // Arrange
            var address = new Address { AddressLine1 = "Line1", Postcode = "Postcode" };
            vacancy.Setup(v => v.EmployerLocationOption).Returns((AvailableWhere?)null);
            vacancy.Setup(v => v.Address).Returns(address);

            // Act
            var result = service.GetVacancyWorkLocation(vacancy.Object);

            // Assert
            result.Should().Be("Line1 (Postcode)");
        }
    }
}
