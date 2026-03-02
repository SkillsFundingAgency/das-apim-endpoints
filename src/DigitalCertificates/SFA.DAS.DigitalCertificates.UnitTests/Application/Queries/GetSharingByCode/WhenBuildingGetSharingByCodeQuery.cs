using System;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.DigitalCertificates.Application.Queries.GetSharingByCode;

namespace SFA.DAS.DigitalCertificates.UnitTests.Application.Queries.GetSharingByCode
{
    public class WhenBuildingGetSharingByCodeQuery
    {
        [Test, AutoData]
        public void Then_Query_Can_Be_Constructed(Guid code)
        {
            var query = new GetSharingByCodeQuery { Code = code };

            query.Code.Should().Be(code);
        }
    }
}
