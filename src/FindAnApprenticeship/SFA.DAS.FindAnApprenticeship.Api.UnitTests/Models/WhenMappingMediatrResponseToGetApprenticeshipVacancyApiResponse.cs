using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Api.Models;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;

namespace SFA.DAS.FindAnApprenticeship.Api.UnitTests.Models
{
    public class WhenMappingMediatrResponseToGetApprenticeshipVacancyApiResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Correctly_Mapped(GetApprenticeshipVacancyQueryResult source)
        {
            var result = (GetApprenticeshipVacancyApiResponse)source;

            using (new AssertionScope())
            {
                result.LongDescription.Should().Be(source.ApprenticeshipVacancy.LongDescription);
                result.OutcomeDescription.Should().Be(source.ApprenticeshipVacancy.OutcomeDescription);

                result.TrainingDescription.Should().Be(source.ApprenticeshipVacancy.TrainingDescription);
                result.ThingsToConsider.Should().Be(source.ApprenticeshipVacancy.ThingsToConsider);
                result.Category.Should().Be(source.ApprenticeshipVacancy.Category);
                result.CategoryCode.Should().Be(source.ApprenticeshipVacancy.CategoryCode);
                result.Description.Should().Be(source.ApprenticeshipVacancy.Description);
                result.FrameworkLarsCode.Should().Be(source.ApprenticeshipVacancy.FrameworkLarsCode);
                result.HoursPerWeek.Should().Be(source.ApprenticeshipVacancy.HoursPerWeek);
                result.IsDisabilityConfident.Should().Be(source.ApprenticeshipVacancy.IsDisabilityConfident);
                result.IsPositiveAboutDisability.Should().Be(source.ApprenticeshipVacancy.IsPositiveAboutDisability);
                result.IsRecruitVacancy.Should().Be(source.ApprenticeshipVacancy.IsRecruitVacancy);
                result.Location.Should().BeEquivalentTo(source.ApprenticeshipVacancy.Location);
                result.NumberOfPositions.Should().Be(source.ApprenticeshipVacancy.NumberOfPositions);
                result.ProviderName.Should().Be(source.ApprenticeshipVacancy.ProviderName);
                result.StandardLarsCode.Should().Be(source.ApprenticeshipVacancy.StandardLarsCode);
                result.StartDate.Should().Be(source.ApprenticeshipVacancy.StartDate);
                result.SubCategory.Should().Be(source.ApprenticeshipVacancy.SubCategory);
                result.SubCategoryCode.Should().Be(source.ApprenticeshipVacancy.SubCategoryCode);
                result.Ukprn.Should().Be(source.ApprenticeshipVacancy.Ukprn);

                result.WageAmountLowerBound.Should().Be(source.ApprenticeshipVacancy.WageAmountLowerBound);
                result.WageAmountUpperBound.Should().Be(source.ApprenticeshipVacancy.WageAmountUpperBound);
                result.WageText.Should().Be(source.ApprenticeshipVacancy.WageText);
                result.WageUnit.Should().Be(source.ApprenticeshipVacancy.WageUnit);
                result.WorkingWeek.Should().Be(source.ApprenticeshipVacancy.WorkingWeek);
                result.ExpectedDuration.Should().Be(source.ApprenticeshipVacancy.ExpectedDuration);
                result.Score.Should().Be(source.ApprenticeshipVacancy.Score);

                result.EmployerDescription.Should().Be(source.ApprenticeshipVacancy.EmployerDescription);
                result.EmployerContactName.Should().Be(source.ApprenticeshipVacancy.EmployerContactName);
                result.EmployerContactEmail.Should().Be(source.ApprenticeshipVacancy.EmployerContactEmail);
                result.EmployerContactPhone.Should().Be(source.ApprenticeshipVacancy.EmployerContactPhone);
                result.EmployerWebsiteUrl.Should().Be(source.ApprenticeshipVacancy.EmployerWebsiteUrl);

                result.VacancyLocationType.Should().Be(source.ApprenticeshipVacancy.VacancyLocationType);
                result.Skills.Should().BeEquivalentTo(source.ApprenticeshipVacancy.Skills);
                result.Qualifications.Should().BeEquivalentTo(source.ApprenticeshipVacancy.Qualifications);

                result.Id.Should().Be(source.ApprenticeshipVacancy.Id);
                result.AnonymousEmployerName.Should().Be(source.ApprenticeshipVacancy.AnonymousEmployerName);
                result.ApprenticeshipLevel.Should().Be(source.ApprenticeshipVacancy.ApprenticeshipLevel);
                result.ClosingDate.Should().Be(source.ApprenticeshipVacancy.ClosingDate);
                result.EmployerName.Should().Be(source.ApprenticeshipVacancy.EmployerName);
                result.IsEmployerAnonymous.Should().Be(source.ApprenticeshipVacancy.IsEmployerAnonymous);
                result.PostedDate.Should().Be(source.ApprenticeshipVacancy.PostedDate);
                result.Title.Should().Be(source.ApprenticeshipVacancy.Title);
                result.VacancyReference.Should().Be(source.ApprenticeshipVacancy.VacancyReference);
                result.CourseTitle.Should().Be(source.ApprenticeshipVacancy.CourseTitle);
                result.CourseId.Should().Be(source.ApprenticeshipVacancy.CourseId);
                result.WageAmount.Should().Be(source.ApprenticeshipVacancy.WageAmount);
                result.WageType.Should().Be(source.ApprenticeshipVacancy.WageType);
                result.Address.Should().BeEquivalentTo(source.ApprenticeshipVacancy.Address);
                result.Distance.Should().Be(source.ApprenticeshipVacancy.Distance);
                result.CourseRoute.Should().Be(source.ApprenticeshipVacancy.CourseRoute);
                result.CourseLevel.Should().Be(source.ApprenticeshipVacancy.CourseLevel);
            }
        }
    }
}
