using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.FindApprenticeshipTraining.Api.Models;
using SFA.DAS.FindApprenticeshipTraining.InnerApi.Responses;

namespace SFA.DAS.FindApprenticeshipTraining.Api.UnitTests.Models
{
    public class WhenCastingGetTrainingCourseProvidersResponseFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetProvidersListItem source)
        {
            var response = (GetTrainingCourseProviderListItem)source;

            response.Name.Should().Be(source.Name);
            response.ProviderId.Should().Be(source.Ukprn);
        }
    }
}