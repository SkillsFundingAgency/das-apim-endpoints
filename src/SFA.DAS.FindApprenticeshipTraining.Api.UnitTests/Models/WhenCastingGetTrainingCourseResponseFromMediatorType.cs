using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.Application.Domain.InnerApi.Types;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetStandardResponse source)
        {
            var response = (GetTrainingCourseResponse)source;

            response.Should().BeEquivalentTo(source);
        }
    }
}
