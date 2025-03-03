using System;
using NUnit.Framework;
using SFA.DAS.ApprenticeApp.Application.Queries.Details;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ApprenticeApp.UnitTests.Handlers
{
    public class GetCategoryArticlesByIdentifierQueryResultTests
    {
        [Test, MoqAutoData]
        public void GetCategoryArticlesByIdentifierQueryResultTestsTestProps()
        {
            var sut = new GetCategoryArticlesByIdentifierQuery
            {
                Slug = "slug",
                Id = new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")
            };

            Assert.That(sut.Slug, Is.EqualTo("slug"));
            Assert.That(sut.Id, Is.EqualTo(new Guid("9D2B0228-4D0D-4C23-8B49-01A698857709")));
        }
    }
}