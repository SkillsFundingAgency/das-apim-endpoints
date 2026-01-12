using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetFrameworkLearner;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetFrameworkLearner
{
    public class WhenBuildingGetFrameworkLearnerQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid frameworkLearnerId)
        {
            // Arrange & Act
            var query = new GetFrameworkLearnerQuery(frameworkLearnerId);

            // Assert
            query.FrameworkLearnerId.Should().Be(frameworkLearnerId);
        }
    }
}
