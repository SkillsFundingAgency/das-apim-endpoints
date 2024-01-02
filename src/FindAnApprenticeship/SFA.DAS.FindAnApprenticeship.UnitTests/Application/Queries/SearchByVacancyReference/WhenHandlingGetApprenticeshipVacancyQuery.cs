using AutoFixture.NUnit3;
using FluentAssertions;
using FluentAssertions.Execution;
using Moq;
using NUnit.Framework;
using SFA.DAS.FindAnApprenticeship.Application.Queries.SearchByVacancyReference;
using SFA.DAS.FindAnApprenticeship.InnerApi.Requests;
using SFA.DAS.FindAnApprenticeship.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.FindAnApprenticeship.UnitTests.Application.Queries.SearchByVacancyReference
{
    public class WhenHandlingGetApprenticeshipVacancyQuery
    {
        [Test, MoqAutoData]
        public async Task Then_The_Services_Are_Called_And_Data_Returned_Based_On_Request(
            GetApprenticeshipVacancyQuery query,
            GetApprenticeshipVacancyItemResponse apiResponse,
            [Frozen] Mock<IFindApprenticeshipApiClient<FindApprenticeshipApiConfiguration>> apiClient,
            GetApprenticeshipVacancyQueryHandler handler)
        {
            // Arrange
            var expectedRequest = new GetVacancyRequest(query.VacancyReference);

            apiClient
                .Setup(client => client.Get<GetApprenticeshipVacancyItemResponse>(It.Is<GetVacancyRequest>(r => r.GetUrl == expectedRequest.GetUrl)))
                .ReturnsAsync(apiResponse);
            
            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            using (new AssertionScope())
            {
                Assert.NotNull(result);
                result.ApprenticeshipVacancy.LongDescription.Should().Be(apiResponse.LongDescription);
                result.ApprenticeshipVacancy.OutcomeDescription.Should().Be(apiResponse.OutcomeDescription);

                result.ApprenticeshipVacancy.TrainingDescription.Should().Be(apiResponse.TrainingDescription);
                result.ApprenticeshipVacancy.ThingsToConsider.Should().Be(apiResponse.ThingsToConsider);
                result.ApprenticeshipVacancy.Category.Should().Be(apiResponse.Category);
                result.ApprenticeshipVacancy.CategoryCode.Should().Be(apiResponse.CategoryCode);
                result.ApprenticeshipVacancy.Description.Should().Be(apiResponse.Description); 
                result.ApprenticeshipVacancy.FrameworkLarsCode.Should().Be(apiResponse.FrameworkLarsCode);
                result.ApprenticeshipVacancy.HoursPerWeek.Should().Be(apiResponse.HoursPerWeek);
                result.ApprenticeshipVacancy.IsDisabilityConfident.Should().Be(apiResponse.IsDisabilityConfident); 
                result.ApprenticeshipVacancy.IsPositiveAboutDisability.Should().Be(apiResponse.IsPositiveAboutDisability);
                result.ApprenticeshipVacancy.IsRecruitVacancy.Should().Be(apiResponse.IsRecruitVacancy);
                result.ApprenticeshipVacancy.Location.Should().Be(apiResponse.Location);
                result.ApprenticeshipVacancy.NumberOfPositions.Should().Be(apiResponse.NumberOfPositions);
                result.ApprenticeshipVacancy.ProviderName.Should().Be(apiResponse.ProviderName); 
                result.ApprenticeshipVacancy.StartDate.Should().Be(apiResponse.StartDate);
                result.ApprenticeshipVacancy.SubCategory.Should().Be(apiResponse.SubCategory);
                result.ApprenticeshipVacancy.SubCategoryCode.Should().Be(apiResponse.SubCategoryCode);
                result.ApprenticeshipVacancy.Ukprn.Should().Be(apiResponse.Ukprn);

                result.ApprenticeshipVacancy.WageAmountLowerBound.Should().Be(apiResponse.WageAmountLowerBound);
                result.ApprenticeshipVacancy.WageAmountUpperBound.Should().Be(apiResponse.WageAmountUpperBound);
                result.ApprenticeshipVacancy.WageText.Should().Be(apiResponse.WageText);
                result.ApprenticeshipVacancy.WageUnit.Should().Be(apiResponse.WageUnit);
                result.ApprenticeshipVacancy.WorkingWeek.Should().Be(apiResponse.WorkingWeek);
                result.ApprenticeshipVacancy.ExpectedDuration.Should().Be(apiResponse.ExpectedDuration);
                result.ApprenticeshipVacancy.Score.Should().Be(apiResponse.Score);

                result.ApprenticeshipVacancy.EmployerDescription.Should().Be(apiResponse.EmployerDescription);
                result.ApprenticeshipVacancy.EmployerContactName.Should().Be(apiResponse.EmployerContactName);
                result.ApprenticeshipVacancy.EmployerContactEmail.Should().Be(apiResponse.EmployerContactEmail);
                result.ApprenticeshipVacancy.EmployerContactPhone.Should().Be(apiResponse.EmployerContactPhone);
                result.ApprenticeshipVacancy.EmployerWebsiteUrl.Should().Be(apiResponse.EmployerWebsiteUrl);

                result.ApprenticeshipVacancy.VacancyLocationType.Should().Be(apiResponse.VacancyLocationType);
                result.ApprenticeshipVacancy.Skills.Should().BeEquivalentTo(apiResponse.Skills);
                result.ApprenticeshipVacancy.Qualifications.Should().BeEquivalentTo(apiResponse.Qualifications);

                result.ApprenticeshipVacancy.Id.Should().Be(apiResponse.Id);
                result.ApprenticeshipVacancy.AnonymousEmployerName.Should().Be(apiResponse.AnonymousEmployerName);
                result.ApprenticeshipVacancy.ApprenticeshipLevel.Should().Be(apiResponse.ApprenticeshipLevel);
                result.ApprenticeshipVacancy.ClosingDate.Should().Be(apiResponse.ClosingDate);
                result.ApprenticeshipVacancy.EmployerName.Should().Be(apiResponse.EmployerName);
                result.ApprenticeshipVacancy.IsEmployerAnonymous.Should().Be(apiResponse.IsEmployerAnonymous);
                result.ApprenticeshipVacancy.PostedDate.Should().Be(apiResponse.PostedDate);
                result.ApprenticeshipVacancy.Title.Should().Be(apiResponse.Title);
                result.ApprenticeshipVacancy.VacancyReference.Should().Be(apiResponse.VacancyReference);
                result.ApprenticeshipVacancy.CourseTitle.Should().Be(apiResponse.CourseTitle);
                result.ApprenticeshipVacancy.CourseId.Should().Be(apiResponse.CourseId);
                result.ApprenticeshipVacancy.WageAmount.Should().Be(apiResponse.WageAmount);
                result.ApprenticeshipVacancy.WageType.Should().Be(apiResponse.WageType);
                result.ApprenticeshipVacancy.Address.Should().Be(apiResponse.Address);
                result.ApprenticeshipVacancy.Distance.Should().Be(apiResponse.Distance);
                result.ApprenticeshipVacancy.CourseRoute.Should().Be(apiResponse.CourseRoute);
                result.ApprenticeshipVacancy.CourseLevel.Should().Be(apiResponse.CourseLevel);
            }
        }
    }
}
