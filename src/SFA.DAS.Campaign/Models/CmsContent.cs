using System;
using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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
        [JsonProperty("description", NullValueHandling = NullValueHandling.Ignore)]
        public string Description { get; set; }

        [JsonProperty("title", NullValueHandling = NullValueHandling.Ignore)]
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
        public NodeType NodeType { get; set; }

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
        public NodeType NodeType { get; set; }
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
        public LinkTypeEnum Type { get; set; }

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
        public PurpleType Type { get; set; }

        [JsonProperty("linkType")]
        public LinkTypeEnum LinkType { get; set; }

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

    public enum NodeType { Hyperlink, Text };

    public enum LinkTypeEnum { ContentType, Entry, Environment, Space };

    public enum PurpleType { Link };

    internal static class Converter
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
            DateParseHandling = DateParseHandling.None,
            Converters =
            {
                NodeTypeConverter.Singleton,
                LinkTypeEnumConverter.Singleton,
                PurpleTypeConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
        };
    }

    internal class NodeTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(NodeType) || t == typeof(NodeType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "hyperlink":
                    return NodeType.Hyperlink;
                case "text":
                    return NodeType.Text;
            }
            throw new Exception("Cannot unmarshal type NodeType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (NodeType)untypedValue;
            switch (value)
            {
                case NodeType.Hyperlink:
                    serializer.Serialize(writer, "hyperlink");
                    return;
                case NodeType.Text:
                    serializer.Serialize(writer, "text");
                    return;
            }
            throw new Exception("Cannot marshal type NodeType");
        }

        public static readonly NodeTypeConverter Singleton = new NodeTypeConverter();
    }

    internal class LinkTypeEnumConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(LinkTypeEnum) || t == typeof(LinkTypeEnum?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            switch (value)
            {
                case "ContentType":
                    return LinkTypeEnum.ContentType;
                case "Entry":
                    return LinkTypeEnum.Entry;
                case "Environment":
                    return LinkTypeEnum.Environment;
                case "Space":
                    return LinkTypeEnum.Space;
            }
            throw new Exception("Cannot unmarshal type LinkTypeEnum");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (LinkTypeEnum)untypedValue;
            switch (value)
            {
                case LinkTypeEnum.ContentType:
                    serializer.Serialize(writer, "ContentType");
                    return;
                case LinkTypeEnum.Entry:
                    serializer.Serialize(writer, "Entry");
                    return;
                case LinkTypeEnum.Environment:
                    serializer.Serialize(writer, "Environment");
                    return;
                case LinkTypeEnum.Space:
                    serializer.Serialize(writer, "Space");
                    return;
            }
            throw new Exception("Cannot marshal type LinkTypeEnum");
        }

        public static readonly LinkTypeEnumConverter Singleton = new LinkTypeEnumConverter();
    }

    internal class PurpleTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(PurpleType) || t == typeof(PurpleType?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            if (value == "Link")
            {
                return PurpleType.Link;
            }
            throw new Exception("Cannot unmarshal type PurpleType");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (PurpleType)untypedValue;
            if (value == PurpleType.Link)
            {
                serializer.Serialize(writer, "Link");
                return;
            }
            throw new Exception("Cannot marshal type PurpleType");
        }

        public static readonly PurpleTypeConverter Singleton = new PurpleTypeConverter();
    }


}