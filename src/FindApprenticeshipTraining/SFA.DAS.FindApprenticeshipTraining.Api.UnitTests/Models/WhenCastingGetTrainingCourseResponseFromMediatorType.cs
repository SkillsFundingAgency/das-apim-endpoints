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
        public void Cast_FromGetStandardsListItem_MapsCommonFields(
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
                .Excluding(tc => tc.IfateReferenceNumber)
                .Excluding(tc => tc.SearchScore)
                .Excluding(tc => tc.RouteCode)
                .Excluding(tc => tc.ApprenticeshipType)
            );

            response.Id.Should().Be(source.LarsCode.ToString());
        }

        [Test, AutoData]
        public void Cast_FromGetStandardsListItemWithMultipleTypicalJobTitles_OrdersTitlesAlphabetically(
            GetStandardsListItem source)
        {
            source.TypicalJobTitles = "B|Z|A|V";

            var expected = new List<string> { "A", "B", "V", "Z" };

            var response = (GetTrainingCourseListItem)source;

            Assert.That(response.TypicalJobTitles, Is.EqualTo(expected));
        }

        [Test, AutoData]
        public void Cast_FromGetStandardsListItemWhenCoreAndOptionsTrue_MapsCoreSkillsFromCoreDuties(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = true;

            var response = (GetTrainingCourseListItem)source;

            response.CoreSkills.Should().BeEquivalentTo(source.CoreDuties);
        }

        [Test, AutoData]
        public void Cast_FromGetStandardsListItemWhenCoreAndOptionsFalse_MapsCoreSkillsFromSkills(
            GetStandardsListItem source)
        {
            source.CoreAndOptions = false;
            var expectedSkills = source.Skills;

            var response = (GetTrainingCourseListItem)source;

            response.CoreSkills.Should().BeEquivalentTo(expectedSkills);
        }
    }
}
