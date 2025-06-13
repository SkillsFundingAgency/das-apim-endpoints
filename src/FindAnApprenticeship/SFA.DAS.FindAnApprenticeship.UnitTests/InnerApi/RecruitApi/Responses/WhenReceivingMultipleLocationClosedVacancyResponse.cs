using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.SharedOuterApi.Domain;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.InnerApi.RecruitApi.Responses;

public class WhenReceivingMultipleLocationClosedVacancyResponse
{
    [Test, MoqAutoData]
    public void Then_One_Location_Addresses_Are_Correct(GetClosedVacancyResponse closedVacancy)
    {
        // arrange
        closedVacancy.EmployerLocation = null;
        closedVacancy.EmployerLocationOption = AvailableWhere.OneLocation;
        closedVacancy.EmployerLocations = [closedVacancy.EmployerLocations!.First()];
        
        // assert
        closedVacancy.Address.Should().BeEquivalentTo(closedVacancy.EmployerLocations!.First());
        closedVacancy.OtherAddresses.Should().BeNullOrEmpty();
    }
    
    [Test, MoqAutoData]
    public void Then_Multiple_Location_Addresses_Are_Correct(GetClosedVacancyResponse closedVacancy)
    {
        // arrange
        closedVacancy.EmployerLocation = null;
        closedVacancy.EmployerLocationOption = AvailableWhere.MultipleLocations;
        var otherAddresses = closedVacancy.EmployerLocations!.Skip(1);
        
        // assert
        closedVacancy.Address.Should().BeEquivalentTo(closedVacancy.EmployerLocations!.First());
        closedVacancy.OtherAddresses.Should().BeEquivalentTo(otherAddresses);
    }
    
    [Test, MoqAutoData]
    public void Then_Multiple_Duplicated_Anonymous_Location_Addresses_Are_Deduplicated_Correctly(GetClosedVacancyResponse closedVacancy)
    {
        // arrange
        closedVacancy.EmployerLocation = null;
        closedVacancy.EmployerLocationOption = AvailableWhere.MultipleLocations;
        closedVacancy.EmployerLocations = [
            new Address { AddressLine3 = "London", Postcode = "SW1AA" },
            new Address { AddressLine3 = "London", Postcode = "SW1AA" },
            new Address { AddressLine3 = "Cambridge", Postcode = "CB24" },
        ];
        
        // assert
        closedVacancy.Address.Should().BeEquivalentTo(closedVacancy.EmployerLocations.First());
        closedVacancy.OtherAddresses.Should().HaveCount(1);
        closedVacancy.OtherAddresses.Should().ContainEquivalentOf(closedVacancy.EmployerLocations.Last());
    }
}