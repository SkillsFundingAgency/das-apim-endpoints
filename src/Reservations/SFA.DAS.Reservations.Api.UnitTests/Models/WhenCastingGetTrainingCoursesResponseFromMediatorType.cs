using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Reservations.Api.Models;
using SFA.DAS.Reservations.InnerApi.Responses;

namespace SFA.DAS.Reservations.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCoursesResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(TrainingCourseListItem source)
        {
            var response = (GetTrainingCoursesListItem)source;

            response.Id.Should().Be(source.LarsCode);
            response.Title.Should().Be(source.Title);
            response.Level.Should().Be(source.Level);
            response.EffectiveTo.Should().Be(source.EffectiveTo);
            response.ApprenticeshipType.Should().Be(source.ApprenticeshipType);
            response.LearningType.Should().Be(source.LearningType);
        }

        [Test]
        public void Then_Maps_ApprenticeshipType_Values_Correctly()
        {
            var source = new TrainingCourseListItem
            {
                LarsCode = "123",
                ApprenticeshipType = "Apprenticeship"
            };
            var response = (GetTrainingCoursesListItem)source;
            response.Id.Should().Be("123");
            response.ApprenticeshipType.Should().Be("Apprenticeship");
        }

        [Test]
        public void Then_Maps_LearningType_And_Id_From_LarsCode_Correctly()
        {
            var source = new TrainingCourseListItem
            {
                LarsCode = "822",
                LearningType = "ApprenticeshipUnit"
            };
            var response = (GetTrainingCoursesListItem)source;
            response.LearningType.Should().Be("ApprenticeshipUnit");
            response.Id.Should().Be("822");
        }
    }
}