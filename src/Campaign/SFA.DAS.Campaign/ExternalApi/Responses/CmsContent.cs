using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Campaign.ExternalApi.Responses
{
   public class CmsContent
    {
        [JsonPropertyName("sys")]
        public SysElement Sys { get; set; }

        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("skip")]
        public long Skip { get; set; }

        [JsonPropertyName("limit")]
        public long Limit { get; set; }

        [JsonPropertyName("items")]
        public List<Item> Items { get; set; }

        [JsonPropertyName("includes")]
        public Includes Includes { get; set; }
    }

    public class Includes
    {
        [JsonPropertyName("Entry")]
        public List<Entry> Entry { get; set; }

        [JsonPropertyName("Asset")]
        public List<Asset> Asset { get; set; }
    }

    public class Asset
    {
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("sys")]
        public AssetSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public AssetFields Fields { get; set; }
    }

    public class AssetFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("file")]
        public File File { get; set; }

        [JsonPropertyName("description")]
        public string Description { get ; set ; }
    }

    public class File
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("details")]
        public Details Details { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }
    }

    public class Details
    {
        [JsonPropertyName("size")]
        public long Size { get; set; }

        [JsonPropertyName("image")]
        public Image Image { get; set; }
    }

    public class Image
    {
        [JsonPropertyName("width")]
        public long Width { get; set; }

        [JsonPropertyName("height")]
        public long Height { get; set; }
    }

    public class Metadata
    {
        [JsonPropertyName("tags")]
        public List<object> Tags { get; set; }
    }

    public class AssetSys
    {
        [JsonPropertyName("space")]
        public LandingPage Space { get; set; }

        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonPropertyName("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonPropertyName("environment")]
        public LandingPage Environment { get; set; }

        [JsonPropertyName("revision")]
        public long Revision { get; set; }

        [JsonPropertyName("locale")]
        public string Locale { get; set; }

        [JsonPropertyName("contentType")]
        public LandingPage ContentType { get; set; }
    }

    public class LandingPage
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class LandingPageSys
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("linkType")]
        public string LinkType { get; set; }
    }

    public class Entry
    {
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("sys")]
        public AssetSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public EntryFields Fields { get; set; }
    }

    public class EntryFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("table")]
        public Table Table { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("hubType")]
        public string HubType { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("headerImage")]
        public LandingPage HeaderImage { get; set; }

        [JsonPropertyName("landingPage")]
        public LandingPage LandingPage { get; set; }

        [JsonPropertyName("content")]
        public PurpleContent Content { get; set; }

        [JsonPropertyName("pageTitle")]
        public string PageTitle { get; set; }

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonPropertyName("tabName")]
        public string TabName { get; set; }


        [JsonPropertyName("tabTitle")]
        public string TabTitle { get; set; }

        [JsonPropertyName("tabContent")]
        public PurpleContent TabContent { get; set; }
        [JsonPropertyName("findTraineeship")]
        public bool FindTraineeship { get; set; }
    }

    public class PurpleContent
    {
        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("data")]
        public PurpleData Data { get; set; }

        [JsonPropertyName("content")]
        public List<FluffyContent> Content { get; set; }
    }

    public class FluffyContent
    {
        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("content")]
        public List<RelatedContent> Content { get; set; }

        [JsonPropertyName("data")]
        public PurpleData Data { get; set; }
    }

    public class RelatedContent
    {
        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("marks")]
        public List<SysElement> Marks { get; set; }

        [JsonPropertyName("data")]
        public RelatedData Data { get; set; }

        [JsonPropertyName("content")]
        public List<RelatedContent> Content { get; set; }
    }

    public class PurpleData
    {
        [JsonPropertyName("target")]
        public LandingPage Target { get; set; }
    }

    public class SysElement
    {
        [JsonPropertyName("type")]
        public string Type { get; set; }
    }

    public class FluffyData
    {
        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }
    }

    public class Table
    {
        [JsonPropertyName("tableData")]
        public List<List<string>> TableData { get; set; }
    }

    public class Item
    {
        [JsonPropertyName("metadata")]
        public Metadata Metadata { get; set; }

        [JsonPropertyName("sys")]
        public AssetSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public ItemFields Fields { get; set; }
    }

    public class ItemFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("hubType")]
        public string HubType { get; set; }

        [JsonPropertyName("landingPage")]
        public LandingPage LandingPage { get; set; }

        [JsonPropertyName("content")]
        public MainContent Content { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("pageTitle")]
        public string PageTitle { get; set; }

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonPropertyName("attachments")]
        public List<LandingPage> Attachments { get; set; }

        [JsonPropertyName("relatedArticles")]
        public List<LandingPage> RelatedArticles { get; set; }

        [JsonPropertyName("headerImage")]
        public HeaderImage HeaderImage { get; set; }

        [JsonPropertyName("cards")]
        public List<CardItem> Cards { get; set; }

        [JsonPropertyName("menuItems")]
        public List<MenuItem> MenuItems { get; set; }

        [JsonPropertyName("tabbedContent")]
        public List<TabbedContent> TabbedContents { get; set; }
        [JsonPropertyName("backgroundColour")]
        public string BackgroundColour { get; set; }

        [JsonPropertyName("allowUserToHideTheBanner")]
        public bool AllowUserToHideTheBanner { get; set; }

        [JsonPropertyName("showOnTheHomepageOnly")]
        public bool ShowOnTheHomepageOnly { get; set; }
        [JsonPropertyName("buttonText")]
        public string ButtonText { get; set; }
        [JsonPropertyName("buttonUrl")]
        public string ButtonUrl { get; set; }
        [JsonPropertyName("buttonStyle")]
        public List<string> ButtonStyle { get; set; }
        [JsonPropertyName("image")]
        public ImageContent Image { get; set; }
    }

    public class MainContent
    {
        [JsonPropertyName("data")]
        public PurpleData Data { get; set; }

        [JsonPropertyName("content")]
        public List<SubContentItems> Content { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }
    }

    public class SubContentItems
    {
        [JsonPropertyName("data")]
        public PurpleData Data { get; set; }

        [JsonPropertyName("content")]
        public List<ContentDefinition> Content { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }
    }

    public class ContentDefinition
    {
        [JsonPropertyName("data")]
        public RelatedData Data { get; set; }

        [JsonPropertyName("marks")]
        public List<Marks> Marks { get; set; }

        [JsonPropertyName("value")]
        public string Value { get; set; }

        [JsonPropertyName("nodeType")]
        public string NodeType { get; set; }

        [JsonPropertyName("content")]
        public List<RelatedContent> Content { get; set; }
    }

    public class RelatedData
    {
        [JsonPropertyName("uri")]
        public Uri Uri { get; set; }

        [JsonPropertyName("target")]
        public LandingPage Target { get; set; }
    }

    public class Marks
    {
        public string Type { get; set; }
    }

    public class HeaderImage
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class CardItem
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class MenuItem
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class TabbedContent
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class ImageContent
    {
        [JsonPropertyName("sys")]
        public LandingPageSys Sys { get; set; }
    }
}
