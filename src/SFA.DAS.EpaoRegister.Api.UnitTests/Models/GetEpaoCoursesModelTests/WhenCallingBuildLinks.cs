using System.Collections.Generic;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.EpaoRegister.Api.Models;
using SFA.DAS.SharedOuterApi.Models;

namespace SFA.DAS.EpaoRegister.Api.UnitTests.Models.GetEpaoCoursesModelTests
{
    public class WhenCallingBuildLinks
    {
        [Test, AutoData]
        public void Then_Builds_Links(
            string expectedGetEpaoUrl,
            string expectedGetEpaoCoursesUrl,
            Mock<IUrlHelper> mockUrlHelper,
            GetEpaoCoursesApiModel model)
        {
            var expectedLinks = new List<Link>
            {
                new Link
                {
                    Rel = "self",
                    Href = expectedGetEpaoCoursesUrl
                },
                new Link
                {
                    Rel = "epao",
                    Href = expectedGetEpaoUrl
                }
            };
            mockUrlHelper.SetupAllEpaoRoutes(null, expectedGetEpaoUrl, expectedGetEpaoCoursesUrl);
            
            model.BuildLinks(mockUrlHelper.Object);

            model.Links.Should().BeEquivalentTo(expectedLinks);
        }
    }
}