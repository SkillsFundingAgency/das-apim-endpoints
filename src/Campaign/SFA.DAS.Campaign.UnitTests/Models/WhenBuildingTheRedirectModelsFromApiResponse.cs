using System.Collections.Generic;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheRedirectModelsFromApiResponse
    {
        [Test]
        public void Then_If_No_Items_Returned_Then_An_Empty_List_Is_Returned()
        {
            var source = new CmsContent { Items = new List<Item>(), Total = 1 };

            var actual = RedirectModel.BuildFrom(source);

            actual.Should().BeEmpty();
        }

        [Test]
        public void Then_If_The_Content_Is_Null_Then_An_Empty_List_Is_Returned()
        {
            var actual = RedirectModel.BuildFrom(null);

            actual.Should().BeEmpty();
        }

        [Test, RecursiveMoqAutoData]
        public void Then_If_Total_Is_Zero_Then_An_Empty_List_Is_Returned(CmsContent source)
        {
            source.Total = 0;

            var actual = RedirectModel.BuildFrom(source);

            actual.Should().BeEmpty();
        }

        [Test]
        public void Then_The_Redirects_Are_Built()
        {
            var source = BuildContent(
                RedirectItem("/employers/old-page", "/employers/new-page", "Exact", true),
                RedirectItem("/employers/retired-section", "/employers", "Prefix", false));

            var actual = RedirectModel.BuildFrom(source);

            actual.Should().BeEquivalentTo(new List<RedirectModel>
            {
                new RedirectModel { FromPath = "/employers/old-page", ToPath = "/employers/new-page", MatchType = "Exact", Permanent = true },
                new RedirectModel { FromPath = "/employers/retired-section", ToPath = "/employers", MatchType = "Prefix", Permanent = false }
            });
        }

        [TestCase("prefix")]
        [TestCase("PREFIX")]
        [TestCase(" Prefix ")]
        public void Then_The_Match_Type_Is_Read_Regardless_Of_Casing_Or_Padding(string matchType)
        {
            var source = BuildContent(RedirectItem("/from", "/to", matchType, true));

            var actual = RedirectModel.BuildFrom(source);

            actual[0].MatchType.Should().Be(RedirectModel.PrefixMatchType);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("something-an-editor-typed")]
        public void Then_An_Unrecognised_Match_Type_Falls_Back_To_Exact(string matchType)
        {
            var source = BuildContent(RedirectItem("/from", "/to", matchType, true));

            var actual = RedirectModel.BuildFrom(source);

            actual[0].MatchType.Should().Be(RedirectModel.ExactMatchType);
        }

        [Test]
        public void Then_An_Unset_Permanent_Flag_Defaults_To_A_Permanent_Redirect()
        {
            var source = BuildContent(RedirectItem("/from", "/to", "Exact", null));

            var actual = RedirectModel.BuildFrom(source);

            actual[0].Permanent.Should().BeTrue();
        }

        [Test]
        public void Then_The_Paths_Are_Trimmed()
        {
            var source = BuildContent(RedirectItem("  /from  ", "  /to  ", "Exact", true));

            var actual = RedirectModel.BuildFrom(source);

            actual[0].FromPath.Should().Be("/from");
            actual[0].ToPath.Should().Be("/to");
        }

        [TestCase(null, "/to")]
        [TestCase("", "/to")]
        [TestCase("  ", "/to")]
        [TestCase("/from", null)]
        [TestCase("/from", "")]
        [TestCase("/from", "  ")]
        public void Then_A_Redirect_Missing_A_Path_Is_Dropped(string fromPath, string toPath)
        {
            var source = BuildContent(
                RedirectItem(fromPath, toPath, "Exact", true),
                RedirectItem("/a-complete/entry", "/a-destination", "Exact", true));

            var actual = RedirectModel.BuildFrom(source);

            actual.Should().HaveCount(1);
            actual[0].FromPath.Should().Be("/a-complete/entry");
        }

        private static CmsContent BuildContent(params Item[] items)
        {
            return new CmsContent { Items = new List<Item>(items), Total = items.Length };
        }

        private static Item RedirectItem(string fromPath, string toPath, string matchType, bool? permanent)
        {
            return new Item
            {
                Fields = new ItemFields
                {
                    FromPath = fromPath,
                    ToPath = toPath,
                    MatchType = matchType,
                    Permanent = permanent
                }
            };
        }
    }
}
