using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingById;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingById
{
    public class WhenBuildingGetSharingByIdQuery
    {
        [Test, AutoData]
        public void Then_Query_Properties_Are_Set_Correctly(Guid sharingId, int limit)
        {
            // Arrange & Act
            var query = new GetSharingByIdQuery
            {
                SharingId = sharingId,
                Limit = limit
            };

            // Assert
            query.SharingId.Should().Be(sharingId);
            query.Limit.Should().Be(limit);
        }
    }
}
