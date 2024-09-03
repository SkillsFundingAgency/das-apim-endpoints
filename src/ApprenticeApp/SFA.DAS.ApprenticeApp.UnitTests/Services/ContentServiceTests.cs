using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Services;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests
{
    public class ContentServiceTests
    {
        [Test, MoqAutoData]
        public Task Get_Categories_By_Content_Type_Test(ContentService service)
        {
            string contentType = "123";
            var result = service.GetCategoriesByContentType<Task>(contentType);
            result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test, MoqAutoData]
        public Task Get_Page_By_Id_Test(ContentService service)
        {
            string id = "123";
            var result = service.GetPageById(id);
            result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test, MoqAutoData]
        public Task Get_Page_By_Id_With_Children_Test(ContentService service)
        {
            string id = "123";
            var result = service.GetPageByIdWithChildren(id);
            result.Should().NotBeNull();
            return Task.CompletedTask;
        }

        [Test, MoqAutoData]
        public Task Get_Category_Articles_By_Identifier_Test(ContentService service)
        {
            string identifier = "123";
            var result = service.GetCategoryArticlesByIdentifier(identifier);
            result.Should().NotBeNull();
            return Task.CompletedTask;
        }
    }
}