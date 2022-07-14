using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseResponseToLevels
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetLevelsListItem source)
        {
            var response = (GetTrainingLevelsListItem) source;
            
            response.Should().BeEquivalentTo(source);
        }
    }
}