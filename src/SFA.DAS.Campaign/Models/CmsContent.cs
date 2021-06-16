using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SFA.DAS.Campaign.Models
{
    public partial class CmsContent
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

    public partial class Includes
    {
        [JsonProperty("Entry")]
        public List<Entry> Entry { get; set; }
    }

    public partial class Entry
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("sys")]
        public EntrySys Sys { get; set; }

        [JsonProperty("fields")]
        public EntryFields Fields { get; set; }
    }

    public partial class EntryFields
    {
        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("hubType", NullValueHandling = NullValueHandling.Ignore)]
        public string HubType { get; set; }

        [JsonProperty("summary", NullValueHandling = NullValueHandling.Ignore)]
        public string Summary { get; set; }

        [JsonProperty("body", NullValueHandling = NullValueHandling.Ignore)]
        public Body Body { get; set; }
    }

    public partial class Body
    {
        [JsonProperty("data")]
        public BodyData Data { get; set; }

        [JsonProperty("content")]
        public List<BodyContent> Content { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }
    }

    public partial class BodyContent
    {
        [JsonProperty("data")]
        public BodyData Data { get; set; }

        [JsonProperty("content")]
        public List<PurpleContent> Content { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }
    }

    public partial class PurpleContent
    {
        [JsonProperty("data")]
        public PurpleData Data { get; set; }

        [JsonProperty("marks", NullValueHandling = NullValueHandling.Ignore)]
        public List<object> Marks { get; set; }

        [JsonProperty("value", NullValueHandling = NullValueHandling.Ignore)]
        public string Value { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }

        [JsonProperty("content", NullValueHandling = NullValueHandling.Ignore)]
        public List<FluffyContent> Content { get; set; }
    }

    public partial class FluffyContent
    {
        [JsonProperty("data")]
        public BodyData Data { get; set; }

        [JsonProperty("marks")]
        public List<SysElement> Marks { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("nodeType")]
        public string NodeType { get; set; }
    }

    public partial class BodyData
    {
    }

    public partial class SysElement
    {
        [JsonProperty("type")]
        public string Type { get; set; }
    }

    public partial class PurpleData
    {
        [JsonProperty("uri", NullValueHandling = NullValueHandling.Ignore)]
        public Uri Uri { get; set; }
    }

    public partial class Metadata
    {
        [JsonProperty("tags")]
        public List<object> Tags { get; set; }
    }

    public partial class EntrySys
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

        [JsonProperty("contentType")]
        public LandingPage ContentType { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }
    }

    public partial class LandingPage
    {
        [JsonProperty("sys")]
        public LandingPageSys Sys { get; set; }
    }

    public partial class LandingPageSys
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("linkType")]
        public string LinkType { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public partial class Item
    {
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        [JsonProperty("sys")]
        public EntrySys Sys { get; set; }

        [JsonProperty("fields")]
        public ItemFields Fields { get; set; }
    }

    public partial class ItemFields
    {
        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("pageTitle")]
        public string PageTitle { get; set; }

        [JsonProperty("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonProperty("hubType")]
        public string HubType { get; set; }

        [JsonProperty("landingPage")]
        public LandingPage LandingPage { get; set; }

        [JsonProperty("summary")]
        public string Summary { get; set; }

        [JsonProperty("sections")]
        public List<LandingPage> Sections { get; set; }
    }

}