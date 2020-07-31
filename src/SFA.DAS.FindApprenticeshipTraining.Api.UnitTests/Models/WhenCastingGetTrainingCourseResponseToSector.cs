using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseResponseToSector
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetSectorsListItem source
        )
        {
            var response = (GetTrainingSectorsListItem) source;
            
            response.Should().BeEquivalentTo(source);
        }
    }
}