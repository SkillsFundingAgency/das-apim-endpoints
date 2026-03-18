using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses;
using SFA.DAS.SharedOuterApi.Types.InnerApi.Responses.Courses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseResponseToSector
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetRoutesListItem source
        )
        {
            var response = (GetTrainingSectorsListItem) source;
            
            response.Id.Should().Be(source.Id);
            response.Route.Should().Be(source.Name);
        }
    }
}