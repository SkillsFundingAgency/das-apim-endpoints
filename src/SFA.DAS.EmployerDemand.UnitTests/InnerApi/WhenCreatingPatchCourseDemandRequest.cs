using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.EmployerDemand.InnerApi.Requests;

namespace SFA.DAS.EmployerDemand.UnitTests.InnerApi
{
    public class WhenCreatingPatchCourseDemandRequest
    {
        [Test, AutoData]
        public void Then_The_Url_Is_Correctly_Constructed(Guid id, PatchOperation data)
        {
            var actual = new PatchCourseDemandRequest(id, data);
            
            actual.Data.Should().BeEquivalentTo(new List<PatchOperation>{data});
            actual.PatchUrl.Should().Be($"api/demand/{id}");
        }
    }
}