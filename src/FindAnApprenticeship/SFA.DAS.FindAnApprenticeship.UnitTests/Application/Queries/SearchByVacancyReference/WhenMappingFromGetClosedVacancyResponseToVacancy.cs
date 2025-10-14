using Humanizer;
using Microsoft.OpenApi.Extensions;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.Domain;
using SFA.DAS.FindAnApprenticeship.Domain.Models;
using SFA.DAS.FindAnApprenticeship.InnerApi.RecruitApi.Responses;
using SFA.DAS.SharedOuterApi.Extensions;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference;

public class WhenMappingFromGetClosedVacancyResponseToVacancy
{
    [Test, AutoData]
    public void Then_The_Fields_Are_Mapped_For_Closed_Vacancy(GetClosedVacancyResponse source)
    {
        // arrange
        source.Wage.DurationUnit = DurationUnit.Year;
        source.ProgrammeId = "1";
        foreach (var sourceQualification in source.Qualifications)
        {
            sourceQualification.Weighting = 0;
        }
        
        // act
        var actual = GetApprenticeshipVacancyQueryResult.Vacancy.FromIVacancy(source);
        
        // assert
        actual.AdditionalTrainingDescription.Should().Be(source.AdditionalTrainingDescription);
        actual.AdditionalQuestion1.Should().Be(source.AdditionalQuestion1);
        actual.AdditionalQuestion2.Should().Be(source.AdditionalQuestion2);
        actual.Address.Should().BeEquivalentTo(source.Address, options => options.ExcludingMissingMembers());
        actual.OtherAddresses.Should().BeEquivalentTo(source.OtherAddresses, options => options
            .Excluding(c =>c.Latitude)
            .Excluding(c => c.Longitude)
        );
        actual.AnonymousEmployerName.Should().Be(source.IsAnonymous ? source.EmployerName : null);
        actual.ApplicationInstructions.Should().Be(source.ApplicationInstructions);
        actual.ApplicationUrl.Should().Be(source.ApplicationUrl);
        actual.ClosingDate.Should().Be(source.ClosedDate ?? source.ClosingDate);
        actual.Description.Should().Be(source.ShortDescription);
        actual.EmployerContactEmail.Should().Be(source.EmployerContact?.Email);
        actual.EmployerContactName.Should().Be(source.EmployerContact?.Name);
        actual.EmployerContactPhone.Should().Be(source.EmployerContact?.Phone);
        actual.EmployerDescription.Should().Be(source.EmployerDescription);
        actual.EmployerName.Should().Be(source.EmployerName);
        actual.EmployerWebsiteUrl.Should().Be(source.EmployerWebsiteUrl);
        actual.ExpectedDuration.Should().Be(((DurationUnit)source.Wage.DurationUnit).GetDisplayName().ToLower().ToQuantity(source.Wage.Duration));
        actual.HoursPerWeek.Should().Be(source.Wage.WeeklyHours);
        actual.Id.Should().Be(source.VacancyReference.TrimVacancyReference());
        actual.IsClosed.Should().Be(source.ClosedDate.HasValue);
        actual.IsDisabilityConfident.Should().Be(source.IsDisabilityConfident);
        actual.IsEmployerAnonymous.Should().Be(source.IsAnonymous);
        actual.IsPositiveAboutDisability.Should().Be(false);
        actual.IsRecruitVacancy.Should().Be(true);
        actual.Location.Lat.Should().Be(source.Address.Latitude);
        actual.Location.Lon.Should().Be(source.Address.Longitude);
        actual.LongDescription.Should().Be(source.Description);
        actual.NumberOfPositions.Should().Be(source.NumberOfPositions);
        actual.OutcomeDescription.Should().Be(source.OutcomeDescription);
        actual.PostedDate.Should().Be(source.LiveDate);
        actual.ProviderContactEmail.Should().Be(source.ProviderContact?.Email);
        actual.ProviderContactName.Should().Be(source.ProviderContact?.Name);
        actual.ProviderContactPhone.Should().Be(source.ProviderContact?.Phone);
        actual.ProviderName.Should().Be(source.TrainingProvider.Name);
        actual.Qualifications.Should().BeEquivalentTo(source.Qualifications, options => options.Excluding(x => x.Weighting));
        actual.Skills.Should().BeEquivalentTo(source.Skills);
        actual.StartDate.Should().Be(source.StartDate);
        actual.ThingsToConsider.Should().Be(source.ThingsToConsider);
        actual.Title.Should().Be(source.Title);
        actual.TrainingDescription.Should().Be(source.TrainingDescription);
        actual.Ukprn.Should().Be(source.TrainingProvider.Ukprn.ToString());
        actual.VacancyLocationType.Should().Be(source.VacancyLocationType);
        actual.VacancyReference.Should().Be(source.VacancyReference.TrimVacancyReference());
        actual.WageAdditionalInformation.Should().Be(source.Wage.WageAdditionalInformation);
        actual.WageType.Should().Be((int)source.Wage.WageType);
        actual.WageUnit.Should().Be((int)source.Wage.DurationUnit);
        actual.WageText.Should().Be(source.Wage.ToDisplayText(source.StartDate));
        actual.WorkingWeek.Should().Be(source.Wage.WorkingWeekDescription);
    }
}