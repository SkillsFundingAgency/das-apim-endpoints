using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingProviderAdditionalCourseListItemFromInnerResponse
    {
        [Test, AutoData]
        public void Then_The_Fields_Are_Mapped(GetAdditionalCourseListItem source)
        {
            var actual = (GetTrainingProviderAdditionalCourseListItem) source;
            
            actual.Should().BeEquivalentTo(source);
        }
    }
}