using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class GetTrainingProviderAdditionalCourseListItemTests
    {
        [Test, AutoData]
        public void WhenMappingGetTrainingProviderAdditionalCourseListItem_ThenTheFieldsAreMapped(GetAdditionalCourseListItem source)
        {
            var actual = (GetTrainingProviderAdditionalCourseListItem)source;

            actual.Should().BeEquivalentTo(source);
        }
    }
}