using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardsListItem source)
        {
            var response = (GetTrainingCourseListItem)source;

            response.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(tc => tc.Skills)
                .Excluding(tc => tc.CoreAndOptions)
                .Excluding(tc => tc.CoreDuties)
                .Excluding(tc => tc.CoreSkillsCount)
            );
        }

        [Test, AutoData]
        public void Then_CoreSkillCount_Is_Set_When_CoreAndOptions_Is_True(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = true;

            var response = (GetTrainingCourseListItem) source;

            response.CoreSkillsCount.Should().BeEquivalentTo(source.CoreDuties);
        }

        [Test, AutoData]
        public void Then_CoreSkillCount_Is_Set_When_CoreAndOptions_Is_False(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = false;
            var expectedSkills = string.Join("|", source.Skills.Select(s => s));

            var response = (GetTrainingCourseListItem)source;

            response.CoreSkillsCount.Should().BeEquivalentTo(expectedSkills);
        }
    }
}
