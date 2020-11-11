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
            );
        }

        [Test, AutoData]
        public void And_If_More_Than_One_Typical_Job_Title_Then_Titles_Are_Ordered_Alphabetically(
            GetStandardsListItem source)
        {
            source.TypicalJobTitles = "B,Z,A,V";

            var response = (GetTrainingCourseListItem) source;

            response.TypicalJobTitles.Should().Be("A,B,V,Z");
        }
    }
}
