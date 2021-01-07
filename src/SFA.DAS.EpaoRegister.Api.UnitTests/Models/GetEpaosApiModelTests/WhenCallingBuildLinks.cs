using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaosApiModelTests
{
    public class WhenCallingBuildLinks
    {
        [Test, AutoData]
        public void Then_Builds_Links(
            string expectedGetEpaosUrl,
            Mock<IUrlHelper> mockUrlHelper,
            GetEpaosApiModel model)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = expectedGetEpaosUrl
                }
            };
            mockUrlHelper.SetupAllEpaoRoutes(expectedGetEpaosUrl, null, null);
            
            model.BuildLinks(mockUrlHelper.Object);

            model.Links.Should().BeEquivalentTo(expectedLinks);
        }

        [Test, AutoData]
        public void Then_Builds_Child_Links(
            string expectedGetEpaoUrl,
            string expectedGetEpaoCoursesUrl,
            Mock<IUrlHelper> mockUrlHelper,
            GetEpaosApiModel model)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = expectedGetEpaoUrl
                },
                new Link
                {
                    Rel = "courses",
                    Href = expectedGetEpaoCoursesUrl
                }
            };
            mockUrlHelper.SetupAllEpaoRoutes(null, expectedGetEpaoUrl, expectedGetEpaoCoursesUrl);

            model.BuildLinks(mockUrlHelper.Object);

            foreach (var epao in model.Epaos)
            {
                epao.Links.Should().BeEquivalentTo(expectedLinks);
            }
        }
    }
}
