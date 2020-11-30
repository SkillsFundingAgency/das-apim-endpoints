using System.Linq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.EpaoRegister.Application.Epaos.Queries.GetEpaos;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaosApiModelTests
{
    public class WhenCastingGetEpaosApiModelFromMediatorType
    {
        [Test, AutoData]
        public void Then_Maps_Fields_Appropriately(
            GetEpaosResult source)
        {
            var response = (GetEpaosApiModel)source;

            response.Epaos.Count().Should().Be(source.Epaos.Count());
        }

        [Test]
        public void And_Source_Is_Null_Then_Returns_Null()
        {
            var response = (GetEpaosApiModel)null;

            response.Should().BeNull();
        }
    }
}