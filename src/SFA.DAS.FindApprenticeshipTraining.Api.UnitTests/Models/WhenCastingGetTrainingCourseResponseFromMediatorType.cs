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

            response.Should().BeEquivalentTo(source, options=> options
                .Excluding(c=>c.ApprenticeshipFunding)
                .Excluding(tc => tc.TypicalJobTitles)
            );
        }

        [Test, AutoData]
        public void And_If_More_Than_One_Typical_Job_Title_Then_Titles_Are_Ordered_Alphabetically(
            GetStandardsListItem source)
        {
            source.TypicalJobTitles = "B|Z|A|V";

            var expected = new List<string>{"A","B","V","Z"};

            var response = (GetTrainingCourseListItem) source;

            Assert.AreEqual(response.TypicalJobTitles, expected);
        }
    }
}
