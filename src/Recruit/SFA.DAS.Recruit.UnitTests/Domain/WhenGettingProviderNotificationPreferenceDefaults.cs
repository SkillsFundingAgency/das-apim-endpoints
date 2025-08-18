using SFA.DAS.Recruit.Domain;

namespace SFA.DAS.Recruit.UnitTests.Domain;

public class WhenGettingProviderNotificationPreferenceDefaults
{
    [Test, MoqAutoData]
    public void Defaults_Should_Be_Added()
    {
        // arrange
        var prefs = new NotificationPreferences();

        // act
        ProviderNotificationPreferences.UpdateWithDefaults(prefs);

        // assert
        prefs.EventPreferences.Should().HaveCount(4);
        var applicationSubmittedPref = prefs.GetForEvent(NotificationTypes.ApplicationSubmitted);
        applicationSubmittedPref.Method.Should().Be("Email");
        applicationSubmittedPref.Scope.Should().Be(NotificationScope.OrganisationVacancies);
        applicationSubmittedPref.Frequency.Should().Be(NotificationFrequency.Daily);
        
        var vacancyApprovedOrRejectedByDfEPref = prefs.GetForEvent(NotificationTypes.VacancyApprovedOrRejected);
        vacancyApprovedOrRejectedByDfEPref.Method.Should().Be("Email");
        vacancyApprovedOrRejectedByDfEPref.Scope.Should().Be(NotificationScope.OrganisationVacancies);
        vacancyApprovedOrRejectedByDfEPref.Frequency.Should().Be(NotificationFrequency.NotSet);
        
        var sharedVacancyPref = prefs.GetForEvent(NotificationTypes.SharedApplicationReviewedByEmployer);
        sharedVacancyPref.Method.Should().Be("Email");
        sharedVacancyPref.Scope.Should().Be(NotificationScope.OrganisationVacancies);
        sharedVacancyPref.Frequency.Should().Be(NotificationFrequency.NotSet);
        
        var providerAttachedPref = prefs.GetForEvent(NotificationTypes.ProviderAttachedToVacancy);
        providerAttachedPref.Method.Should().Be("Email");
        providerAttachedPref.Scope.Should().Be(NotificationScope.OrganisationVacancies);
        providerAttachedPref.Frequency.Should().Be(NotificationFrequency.Immediately);
    }
    
    [Test, MoqAutoData]
    public void Defaults_Should_Not_Override_Existing()
    {
        // arrange
        var existingPref = new NotificationPreference(NotificationTypes.ApplicationSubmitted, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Immediately);
        var prefs = new NotificationPreferences
        {
            EventPreferences = [ existingPref ]
        };

        // act
        ProviderNotificationPreferences.UpdateWithDefaults(prefs);

        // assert
        prefs.EventPreferences.Should().HaveCount(4);
        var applicationSubmittedPref = prefs.GetForEvent(NotificationTypes.ApplicationSubmitted);
        applicationSubmittedPref.Should().BeEquivalentTo(existingPref);
    }
    
    [Test, MoqAutoData]
    public void Defaults_Should_Not_Remove_Other_Existing_Preferences_That_Are_Not_Defaults()
    {
        // arrange
        var existingPref = new NotificationPreference(NotificationTypes.VacancyClosingSoon, "Email", NotificationScope.OrganisationVacancies, NotificationFrequency.Immediately);
        var prefs = new NotificationPreferences
        {
            EventPreferences = [ existingPref ]
        };

        // act
        ProviderNotificationPreferences.UpdateWithDefaults(prefs);

        // assert
        prefs.EventPreferences.Should().HaveCount(5);
        var pref = prefs.GetForEvent(NotificationTypes.VacancyClosingSoon);
        pref.Should().Be(existingPref);
    }
}