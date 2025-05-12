using System;
using System.Threading.Tasks;
using AutoFixture.NUnit3;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Api.Controllers;
using SFA.DAS.Testing.AutoFixture;
using static SFA.DAS.ApprenticeApp.Api.Controllers.SupportAndGuidanceController;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class SupportAndGuidanceControllerTests
    {
        [Test, MoqAutoData]
        public async Task Get_Categories_Test(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            string contentType = "1234";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetCategories(contentType);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Articles_For_Category_Tests(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            string categoryIdentifier = "1234";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetArticlesForCategory(categoryIdentifier, apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_One_By_Entry_Id(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();
            string entryId = "1234";

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetOneByEntryId(entryId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Get_Saved_Articles_tests(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();
            var apprenticeId = Guid.NewGuid();

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.GetSavedArticles(apprenticeId);
            result.Should().BeOfType(typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
        }

        [Test, MoqAutoData]
        public async Task Add_Update_Apprentice_Article_Test(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();

            var id = Guid.NewGuid();
            var articleIdentifier = "1234";
            var articleTitle = "title";
            var request = new ApprenticeArticleRequest() { IsSaved = true, LikeStatus = true };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.AddUpdateApprenticeArticle(id, articleIdentifier, articleTitle, request) as OkResult;
            result.Should().BeOfType(typeof(OkResult));
        }

        [Test, MoqAutoData]
        public async Task Remove_Apprentice_Article_Test(
            [Greedy] SupportAndGuidanceController controller)
        {
            var httpContext = new DefaultHttpContext();

            var id = Guid.NewGuid();
            var articleIdentifier = "1234";
            var request = new ApprenticeArticleRequest() { IsSaved = true, LikeStatus = true };

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var result = await controller.RemoveApprenticeArticle(id, articleIdentifier, request) as OkResult;
            result.Should().BeOfType(typeof(OkResult));
        }
    }
}