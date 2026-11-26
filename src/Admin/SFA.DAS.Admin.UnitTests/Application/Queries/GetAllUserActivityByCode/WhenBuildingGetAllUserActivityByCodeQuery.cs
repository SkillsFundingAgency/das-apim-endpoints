using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetAllUserActivityByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetAllUserActivityByCode
{
    public class WhenBuildingGetAllUserActivityByCodeQuery
    {
        [Test, AutoData]
        public void Then_Query_Can_Be_Constructed(string code)
        {
            var query = new GetAllUserActivityByCodeQuery { Code = code };

            query.Code.Should().Be(code);
        }
    }
}
