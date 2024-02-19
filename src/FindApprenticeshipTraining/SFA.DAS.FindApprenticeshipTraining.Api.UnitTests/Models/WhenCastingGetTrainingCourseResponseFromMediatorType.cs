using System.Collections.Generic;
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

            response.Should().BeEquivalentTo(source, options => options
                .Excluding(c => c.ApprenticeshipFunding)
                .Excluding(tc => tc.Skills)
                .Excluding(tc => tc.TypicalJobTitles)
                .Excluding(tc => tc.CoreAndOptions)
                .Excluding(tc => tc.CoreDuties)
                .Excluding(tc => tc.CoreSkills)
                .Excluding(tc => tc.IsActive)
                .Excluding(tc => tc.LarsCode)
                .Excluding(tc => tc.StandardUId)
                .Excluding(tc => tc.SectorSubjectAreaTier1)
            );

            response.Id.Should().Be(source.LarsCode);
        }

        [Test, AutoData]
        public void And_If_More_Than_One_Typical_Job_Title_Then_Titles_Are_Ordered_Alphabetically(
            GetStandardsListItem source)
        {
            source.TypicalJobTitles = "B|Z|A|V";

            var expected = new List<string> { "A", "B", "V", "Z" };

            var response = (GetTrainingCourseListItem)source;

            Assert.AreEqual(response.TypicalJobTitles, expected);
        }

        [Test, AutoData]
        public void Then_CoreSkillCount_Is_Set_When_CoreAndOptions_Is_True(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = true;

            var response = (GetTrainingCourseListItem)source;

            response.CoreSkills.Should().BeEquivalentTo(source.CoreDuties);
        }
        [Test, AutoData]
        public void Then_CoreSkillCount_Is_Set_When_CoreAndOptions_Is_False(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = false;
            var expectedSkills = source.Skills;

            var response = (GetTrainingCourseListItem)source;

            response.CoreSkills.Should().BeEquivalentTo(expectedSkills);
        }
    }
}
