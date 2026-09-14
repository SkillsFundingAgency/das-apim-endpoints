using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Admin.Application.Queries.GetUserActionByCode;

namespace SFA.DAS.Admin.UnitTests.Application.Queries.GetUserActionByCode
{
    public class WhenBuildingGetUserActionByCodeQuery
    {
        [Test, AutoData]
        public void Then_Query_Can_Be_Constructed(string code)
        {
            var query = new GetUserActionByCodeQuery { Code = code };

            query.Code.Should().Be(code);
        }
    }
}
