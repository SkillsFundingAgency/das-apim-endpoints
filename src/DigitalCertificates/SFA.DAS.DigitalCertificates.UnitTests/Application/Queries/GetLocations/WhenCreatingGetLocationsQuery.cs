using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetLocations;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetLocations
{
    public class WhenCreatingGetLocationsQuery
    {
        [Test, MoqAutoData]
        public void Then_Query_Property_Can_Be_Set(string queryString)
        {
            var query = new GetLocationsQuery { Query = queryString };

            query.Query.Should().Be(queryString);
        }
    }
}
