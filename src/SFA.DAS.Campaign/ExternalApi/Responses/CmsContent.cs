using System;
using System.Collections.Generic;
using Contentful.Core.Models;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.ExternalApi.Responses
{
   public class CmsContent
    {
        [JsonProperty("sys")]
        public SysElement Sys { get; set; }

        [JsonProperty("total")]
        public long Total { get; set; }

        [JsonProperty("skip")]
        public long Skip { get; set; }

        [JsonProperty("limit")]
        public long Limit { get; set; }

        [JsonProperty("items")]
        public List<Item> Items { get; set; }

        [JsonProperty("includes")]
        public Includes Includes { get; set; }
    }

    public class Includes
    {
        [JsonProperty("Entry")]
        public List<Entry> Entry { get; set; }

        [JsonProperty("Asset")]
        public List<Asset> Asset { get; set; }
    }

    public class Asset
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("sys")]
        public AssetSys Sys { get; set; }

        [JsonProperty("fields")]
        public AssetFields Fields { get; set; }
    }

    public class AssetFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("file")]
        public File File { get; set; }

        [JsonProperty("description")]
        public string Description { get ; set ; }
    }

    public class File
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("details")]
        public Details Details { get; set; }

        [JsonProperty("fileName")]
        public string FileName { get; set; }

        [JsonProperty("contentType")]
        public string ContentType { get; set; }
    }

    public class Details
    {
        [JsonProperty("size")]
        public long Size { get; set; }

        [JsonProperty("image", NullValueHandling = NullValueHandling.Ignore)]
        public Image Image { get; set; }
    }

    public class Image
    {
        [JsonProperty("width")]
        public long Width { get; set; }

        [JsonProperty("height")]
        public long Height { get; set; }
    }

    public class Metadata
    {
        [JsonProperty("tags")]
        public List<object> Tags { get; set; }
    }

    public class AssetSys
    {
        [JsonProperty("space")]
        public LandingPage Space { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty("environment")]
        public LandingPage Environment { get; set; }

        [JsonProperty("revision")]
        public long Revision { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("contentType", NullValueHandling = NullValueHandling.Ignore)]
        public LandingPage ContentType { get; set; }
    }

    public class LandingPage
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class LandingPageSys
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("linkType")]
        public string LinkType { get; set; }
    }

    public class Entry
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("sys")]
        public AssetSys Sys { get; set; }

        [JsonProperty("fields")]
        public EntryFields Fields { get; set; }
    }

    public class EntryFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("table", NullValueHandling = NullValueHandling.Ignore)]
        public Table Table { get; set; }

        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("slug", NullValueHandling = NullValueHandling.Ignore)]
        public string Slug { get; set; }

        [JsonProperty("hubType", NullValueHandling = NullValueHandling.Ignore)]
        public string HubType { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }

        [JsonProperty("headerImage", NullValueHandling = NullValueHandling.Ignore)]
        public LandingPage HeaderImage { get; set; }

        [JsonProperty("landingPage", NullValueHandling = NullValueHandling.Ignore)]
        public LandingPage LandingPage { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleContent Content { get; set; }

        [JsonProperty("pageTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string PageTitle { get; set; }

        [JsonProperty("metaDescription", NullValueHandling = NullValueHandling.Ignore)]
        public string MetaDescription { get; set; }

        [JsonProperty("tabName", NullValueHandling = NullValueHandling.Ignore)]
        public string TabName { get; set; }


        [JsonProperty("tabTitle", NullValueHandling = NullValueHandling.Ignore)]
        public string TabTitle { get; set; }

        [JsonProperty("tabContent", NullValueHandling = NullValueHandling.Ignore)]
        public PurpleContent TabContent { get; set; }
        [JsonProperty("findTraineeship")]
        public bool FindTraineeship { get; set; }
    }

    public class PurpleContent
    {
        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("data")]
        public PurpleData Data { get; set; }

        [JsonProperty("content")]
        public List<FluffyContent> Content { get; set; }
    }

    public class FluffyContent
    {
        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("content")]
        public List<RelatedContent> Content { get; set; }

        [JsonProperty("data")]
        public PurpleData Data { get; set; }
    }

    public class RelatedContent
    {
        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("marks", NullValueHandling = NullValueHandling.Ignore)]
        public List<SysElement> Marks { get; set; }

        [JsonProperty("data")]
        public RelatedData Data { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public List<RelatedContent> Content { get; set; }
    }

    public class PurpleData
    {
        [JsonProperty("target")]
        public LandingPage Target { get; set; }
    }

    public class SysElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public class FluffyData
    {
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Uri { get; set; }
    }

    public class Table
    {
        [JsonProperty("tableData")]
        public List<List<string>> TableData { get; set; }
    }

    public class Item
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("sys")]
        public AssetSys Sys { get; set; }

        [JsonProperty("fields")]
        public ItemFields Fields { get; set; }
    }

    public class ItemFields
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("hubType")]
        public string HubType { get; set; }

        [JsonProperty("landingPage")]
        public LandingPage LandingPage { get; set; }

        [JsonProperty("content")]
        public MainContent Content { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("pageTitle")]
        public string PageTitle { get; set; }

        [JsonProperty("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonProperty("attachments")]
        public List<LandingPage> Attachments { get; set; }

        [JsonProperty("relatedArticles")]
        public List<LandingPage> RelatedArticles { get; set; }

        [JsonProperty("headerImage")]
        public HeaderImage HeaderImage { get; set; }

        [JsonProperty("cards")]
        public List<CardItem> Cards { get; set; }

        [JsonProperty("menuItems")]
        public List<MenuItem> MenuItems { get; set; }

        [JsonProperty("tabbedContent")]
        public List<TabbedContent> TabbedContents { get; set; }
        [JsonProperty("backgroundColour", NullValueHandling = NullValueHandling.Ignore)]
        public string BackgroundColour { get; set; }

        [JsonProperty("allowUserToHideTheBanner", NullValueHandling = NullValueHandling.Ignore)]
        public bool AllowUserToHideTheBanner { get; set; }

        [JsonProperty("showOnTheHomepageOnly", NullValueHandling = NullValueHandling.Ignore)]
        public bool ShowOnTheHomepageOnly { get; set; }
    }

    public class MainContent
    {
        [JsonProperty("data")]
        public PurpleData Data { get; set; }

        [JsonProperty("content")]
        public List<SubContentItems> Content { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }
    }

    public class SubContentItems
    {
        [JsonProperty("data")]
        public PurpleData Data { get; set; }

        [JsonProperty("content")]
        public List<ContentDefinition> Content { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }
    }

    public class ContentDefinition
    {
        [JsonProperty("data")]
        public RelatedData Data { get; set; }

        [JsonProperty("marks", NullValueHandling = NullValueHandling.Ignore)]
        public List<Marks> Marks { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public List<RelatedContent> Content { get; set; }
    }

    public class RelatedData
    {
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Uri { get; set; }

        [JsonProperty("target", NullValueHandling = NullValueHandling.Ignore)]
        public LandingPage Target { get; set; }
    }

    public class Marks
    {
        public string Type { get; set; }
    }

    public class HeaderImage
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class CardItem
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class MenuItem
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public class TabbedContent
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }
}
