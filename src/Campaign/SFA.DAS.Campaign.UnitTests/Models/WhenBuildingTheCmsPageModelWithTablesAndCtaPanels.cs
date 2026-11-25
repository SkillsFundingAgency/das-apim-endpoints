using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.Campaign.ExternalApi.Responses;
using SFA.DAS.Campaign.Models;

namespace SFA.DAS.Campaign.UnitTests.Models
{
    public class WhenBuildingTheCmsPageModelWithTablesAndCtaPanels
    {
        private const string Table = @"{
            ""nodeType"": ""table"", ""data"": {}, ""content"": [
                { ""nodeType"": ""table-row"", ""data"": {}, ""content"": [
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Level"", ""marks"": [], ""data"": {} } ] } ] },
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Equivalent"", ""marks"": [], ""data"": {} } ] } ] } ] },
                { ""nodeType"": ""table-row"", ""data"": {}, ""content"": [
                    { ""nodeType"": ""table-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Level 2"", ""marks"": [ { ""type"": ""bold"" } ], ""data"": {} } ] } ] },
                    { ""nodeType"": ""table-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""See "", ""marks"": [], ""data"": {} },
                            { ""nodeType"": ""hyperlink"", ""data"": { ""uri"": ""https://www.gov.uk/"" }, ""content"": [
                                { ""nodeType"": ""text"", ""value"": ""GCSE"", ""marks"": [], ""data"": {} } ] } ] },
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Second line"", ""marks"": [], ""data"": {} } ] } ] } ] } ] }";

        private const string TableWithHeaderColumn = @"{
            ""nodeType"": ""table"", ""data"": {}, ""content"": [
                { ""nodeType"": ""table-row"", ""data"": {}, ""content"": [
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Level 2"", ""marks"": [], ""data"": {} } ] } ] },
                    { ""nodeType"": ""table-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""GCSE"", ""marks"": [], ""data"": {} } ] } ] } ] },
                { ""nodeType"": ""table-row"", ""data"": {}, ""content"": [
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""Level 3"", ""marks"": [], ""data"": {} } ] } ] },
                    { ""nodeType"": ""table-cell"", ""data"": {}, ""content"": [
                        { ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                            { ""nodeType"": ""text"", ""value"": ""A level"", ""marks"": [], ""data"": {} } ] } ] } ] } ] }";

        private const string CtaPanelBlock = @"{
            ""nodeType"": ""embedded-entry-block"", ""content"": [],
            ""data"": { ""target"": { ""sys"": { ""id"": ""cta1"", ""type"": ""Link"", ""linkType"": ""Entry"" } } } }";

        private const string CtaPanelInline = @"{
            ""nodeType"": ""paragraph"", ""data"": {}, ""content"": [
                { ""nodeType"": ""text"", ""value"": """", ""marks"": [], ""data"": {} },
                { ""nodeType"": ""embedded-entry-inline"", ""content"": [],
                  ""data"": { ""target"": { ""sys"": { ""id"": ""cta1"", ""type"": ""Link"", ""linkType"": ""Entry"" } } } },
                { ""nodeType"": ""text"", ""value"": """", ""marks"": [], ""data"": {} } ] }";

        private static readonly List<List<string>> ExpectedTable = new List<List<string>>
        {
            new List<string> { "Level", "Equivalent" },
            new List<string> { "[bold]Level 2", "See [GCSE](https://www.gov.uk/)\nSecond line" }
        };

        private static readonly CtaPanelModel ExpectedCtaPanel = new CtaPanelModel
        {
            Heading = "Ready to hire?",
            Description = "Find out how",
            Icon = "arrow",
            ButtonText = "Get started",
            Url = "/employers/get-started"
        };

        [Test]
        public void Then_A_Table_In_Tabbed_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(tabContent: Table);

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items.Should().ContainSingle();
            items[0].Type.Should().Be("table");
            items[0].TableValue.Should().BeEquivalentTo(ExpectedTable, options => options.WithStrictOrdering());
            items[0].TableHasHeaderRow.Should().BeTrue();
            items[0].TableHasHeaderColumn.Should().BeFalse();
        }

        [Test]
        public void Then_A_Table_With_A_Header_Column_In_Tabbed_Content_Is_Flagged_As_Having_One()
        {
            var source = BuildSource(tabContent: TableWithHeaderColumn);

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items[0].TableValue.Should().BeEquivalentTo(new List<List<string>>
            {
                new List<string> { "Level 2", "GCSE" },
                new List<string> { "Level 3", "A level" }
            }, options => options.WithStrictOrdering());
            items[0].TableHasHeaderColumn.Should().BeTrue();
            items[0].TableHasHeaderRow.Should().BeFalse();
        }

        [Test]
        public void Then_A_Table_With_A_Header_Column_In_The_Main_Content_Is_Flagged_As_Having_One()
        {
            var source = BuildSource(mainContent: TableWithHeaderColumn);

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items[0].TableHasHeaderColumn.Should().BeTrue();
            actual.MainContent.Items[0].TableHasHeaderRow.Should().BeFalse();
        }

        [Test]
        public void Then_A_Table_With_A_Header_Row_And_Header_Column_Is_Flagged_As_Having_Both()
        {
            var headerRow = @"{ ""nodeType"": ""table-row"", ""data"": {}, ""content"": [
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [] },
                    { ""nodeType"": ""table-header-cell"", ""data"": {}, ""content"": [] } ] },";
            var table = TableWithHeaderColumn.Replace(@"""content"": [
                { ""nodeType"": ""table-row""", $@"""content"": [
                {headerRow}
                {{ ""nodeType"": ""table-row""");
            var source = BuildSource(tabContent: table);

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items[0].TableValue.Count.Should().Be(3);
            items[0].TableHasHeaderRow.Should().BeTrue();
            items[0].TableHasHeaderColumn.Should().BeTrue();
        }

        [Test]
        public void Then_A_Table_Without_A_Header_Row_In_Tabbed_Content_Is_Not_Flagged_As_Having_One()
        {
            var source = BuildSource(tabContent: Table.Replace("table-header-cell", "table-cell"));

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items[0].TableValue.Should().BeEquivalentTo(ExpectedTable, options => options.WithStrictOrdering());
            items[0].TableHasHeaderRow.Should().BeFalse();
            items[0].TableHasHeaderColumn.Should().BeFalse();
        }

        [Test]
        public void Then_A_Table_Without_A_Header_Row_In_The_Main_Content_Is_Not_Flagged_As_Having_One()
        {
            var source = BuildSource(mainContent: Table.Replace("table-header-cell", "table-cell"));

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items[0].TableHasHeaderRow.Should().BeFalse();
        }

        [Test]
        public void Then_A_Table_In_The_Main_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(mainContent: Table);

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items.Should().ContainSingle();
            actual.MainContent.Items[0].Type.Should().Be("table");
            actual.MainContent.Items[0].TableValue.Should().BeEquivalentTo(ExpectedTable, options => options.WithStrictOrdering());
            actual.MainContent.Items[0].TableHasHeaderRow.Should().BeTrue();
        }

        [Test]
        public void Then_A_Cta_Panel_Block_In_Tabbed_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(tabContent: CtaPanelBlock);

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items.Should().ContainSingle();
            items[0].Type.Should().Be("embedded-entry-block");
            items[0].CtaPanel.Should().BeEquivalentTo(ExpectedCtaPanel);
        }

        [Test]
        public void Then_A_Cta_Panel_Block_In_The_Main_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(mainContent: CtaPanelBlock);

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items.Should().ContainSingle();
            actual.MainContent.Items[0].Type.Should().Be("embedded-entry-block");
            actual.MainContent.Items[0].CtaPanel.Should().BeEquivalentTo(ExpectedCtaPanel);
        }

        [Test]
        public void Then_An_Inline_Cta_Panel_In_Tabbed_Content_Is_Added_To_The_Paragraph()
        {
            var source = BuildSource(tabContent: CtaPanelInline);

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items.Should().ContainSingle();
            items[0].Type.Should().Be("paragraph");
            items[0].CtaPanel.Should().BeEquivalentTo(ExpectedCtaPanel);
        }

        [Test]
        public void Then_An_Inline_Cta_Panel_In_The_Main_Content_Is_Added_To_The_Paragraph()
        {
            var source = BuildSource(mainContent: CtaPanelInline);

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items.Should().ContainSingle();
            actual.MainContent.Items[0].Type.Should().Be("paragraph");
            actual.MainContent.Items[0].CtaPanel.Should().BeEquivalentTo(ExpectedCtaPanel);
        }

        private static readonly StatsSectionModel ExpectedStatsSection = new StatsSectionModel
        {
            Text = "of employers said apprentices improved productivity",
            HighlightValue = "86%",
            QuoteName = "Jane Smith",
            QuoteRole = "Managing Director",
            ReferenceText = "Employer survey 2025"
        };

        [Test]
        public void Then_A_Stats_Section_Block_In_Tabbed_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(tabContent: CtaPanelBlock.Replace("cta1", "stats1"));

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items.Should().ContainSingle();
            items[0].Type.Should().Be("embedded-entry-block");
            items[0].StatsSection.Should().BeEquivalentTo(ExpectedStatsSection);
            items[0].CtaPanel.Should().BeNull();
        }

        [Test]
        public void Then_A_Stats_Section_Block_In_The_Main_Content_Is_Added_To_The_Content_Items()
        {
            var source = BuildSource(mainContent: CtaPanelBlock.Replace("cta1", "stats1"));

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items.Should().ContainSingle();
            actual.MainContent.Items[0].Type.Should().Be("embedded-entry-block");
            actual.MainContent.Items[0].StatsSection.Should().BeEquivalentTo(ExpectedStatsSection);
            actual.MainContent.Items[0].CtaPanel.Should().BeNull();
        }

        [Test]
        public void Then_An_Inline_Stats_Section_In_Tabbed_Content_Is_Added_To_The_Paragraph()
        {
            var source = BuildSource(tabContent: CtaPanelInline.Replace("cta1", "stats1"));

            var actual = new CmsPageModel().Build(source, null, null);

            var items = actual.TabbedContents.Single().Content.Items;
            items.Should().ContainSingle();
            items[0].Type.Should().Be("paragraph");
            items[0].StatsSection.Should().BeEquivalentTo(ExpectedStatsSection);
            items[0].CtaPanel.Should().BeNull();
        }

        [Test]
        public void Then_An_Inline_Stats_Section_In_The_Main_Content_Is_Added_To_The_Paragraph()
        {
            var source = BuildSource(mainContent: CtaPanelInline.Replace("cta1", "stats1"));

            var actual = new CmsPageModel().Build(source, null, null);

            actual.MainContent.Items.Should().ContainSingle();
            actual.MainContent.Items[0].Type.Should().Be("paragraph");
            actual.MainContent.Items[0].StatsSection.Should().BeEquivalentTo(ExpectedStatsSection);
            actual.MainContent.Items[0].CtaPanel.Should().BeNull();
        }

        [Test]
        public void Then_A_Cta_Panel_Has_No_Stats_Section()
        {
            var source = BuildSource(tabContent: CtaPanelBlock);

            var actual = new CmsPageModel().Build(source, null, null);

            actual.TabbedContents.Single().Content.Items.Single().StatsSection.Should().BeNull();
        }

        [Test]
        public void Then_An_Embedded_Entry_Block_That_Is_Not_A_Cta_Panel_Is_Not_Added()
        {
            var source = BuildSource(tabContent: CtaPanelBlock.Replace("cta1", "tab1"));

            var actual = new CmsPageModel().Build(source, null, null);

            actual.TabbedContents.Single().Content.Items.Should().BeEmpty();
        }

        [Test]
        public void Then_Paragraphs_Without_A_Cta_Panel_Have_No_Cta_Panel()
        {
            var source = BuildSource(tabContent: CtaPanelInline.Replace("cta1", "missing"));

            var actual = new CmsPageModel().Build(source, null, null);

            actual.TabbedContents.Single().Content.Items.Single().CtaPanel.Should().BeNull();
        }

        private static CmsContent BuildSource(string mainContent = null, string tabContent = null)
        {
            var json = $@"{{
                ""total"": 1,
                ""items"": [ {{
                    ""sys"": {{ ""id"": ""article1"", ""contentType"": {{ ""sys"": {{ ""id"": ""article"", ""type"": ""Link"", ""linkType"": ""ContentType"" }} }} }},
                    ""fields"": {{
                        ""title"": ""Article"", ""slug"": ""article"", ""hubType"": ""Employers"",
                        ""content"": {{ ""nodeType"": ""document"", ""data"": {{}}, ""content"": [ {mainContent} ] }},
                        ""tabbedContent"": [ {{ ""sys"": {{ ""id"": ""tab1"", ""type"": ""Link"", ""linkType"": ""Entry"" }} }} ]
                    }} }} ],
                ""includes"": {{ ""Entry"": [
                    {{
                        ""sys"": {{ ""id"": ""tab1"", ""contentType"": {{ ""sys"": {{ ""id"": ""tab"", ""type"": ""Link"", ""linkType"": ""ContentType"" }} }} }},
                        ""fields"": {{
                            ""tabName"": ""Tab"", ""tabTitle"": ""Tab"",
                            ""tabContent"": {{ ""nodeType"": ""document"", ""data"": {{}}, ""content"": [ {tabContent} ] }} }}
                    }},
                    {{
                        ""sys"": {{ ""id"": ""cta1"", ""contentType"": {{ ""sys"": {{ ""id"": ""ctaPanel"", ""type"": ""Link"", ""linkType"": ""ContentType"" }} }} }},
                        ""fields"": {{
                            ""heading"": ""Ready to hire?"", ""description"": ""Find out how"", ""icon"": ""arrow"",
                            ""buttonText"": ""Get started"", ""url"": ""/employers/get-started"" }}
                    }},
                    {{
                        ""sys"": {{ ""id"": ""stats1"", ""contentType"": {{ ""sys"": {{ ""id"": ""statsSection"", ""type"": ""Link"", ""linkType"": ""ContentType"" }} }} }},
                        ""fields"": {{
                            ""text"": ""of employers said apprentices improved productivity"", ""highlightValue"": ""86%"",
                            ""quoteName"": ""Jane Smith"", ""quoteRole"": ""Managing Director"", ""referenceText"": ""Employer survey 2025"" }}
                    }} ] }}
            }}";

            return JsonSerializer.Deserialize<CmsContent>(json);
        }
    }
}
