using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace SFA.DAS.Campaign.ExternalApi.Responses
{
    /// <summary>
    /// A narrowed view of the Contentful entries response for hub pages. Only the fields
    /// <see cref="Models.HubPageModel"/> maps are declared, so linked article and tab bodies are
    /// discarded on deserialisation rather than being carried through the cache.
    /// </summary>
    public class HubCmsContent
    {
        [JsonPropertyName("total")]
        public long Total { get; set; }

        [JsonPropertyName("items")]
        public List<HubItem> Items { get; set; }

        [JsonPropertyName("includes")]
        public HubIncludes Includes { get; set; }
    }

    public class HubIncludes
    {
        [JsonPropertyName("Entry")]
        public List<HubEntry> Entry { get; set; }

        [JsonPropertyName("Asset")]
        public List<HubAsset> Asset { get; set; }
    }

    public class HubItem
    {
        [JsonPropertyName("sys")]
        public HubSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public HubItemFields Fields { get; set; }
    }

    public class HubEntry
    {
        [JsonPropertyName("sys")]
        public HubSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public HubEntryFields Fields { get; set; }
    }

    public class HubAsset
    {
        [JsonPropertyName("sys")]
        public HubSys Sys { get; set; }

        [JsonPropertyName("fields")]
        public HubAssetFields Fields { get; set; }
    }

    public class HubSys
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("contentType")]
        public HubLink ContentType { get; set; }
    }

    public class HubLink
    {
        [JsonPropertyName("sys")]
        public HubLinkSys Sys { get; set; }
    }

    public class HubLinkSys
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("linkType")]
        public string LinkType { get; set; }
    }

    public class HubItemFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("hubType")]
        public string HubType { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonPropertyName("headerImage")]
        public HubLink HeaderImage { get; set; }

        [JsonPropertyName("cardsTitle")]
        public string CardsTitle { get; set; }

        [JsonPropertyName("cards")]
        public List<HubLink> Cards { get; set; }

        [JsonPropertyName("cardsTitle2")]
        public string CardsTitle2 { get; set; }

        [JsonPropertyName("cards2")]
        public List<HubLink> Cards2 { get; set; }

        [JsonPropertyName("cardsTitle3")]
        public string CardsTitle3 { get; set; }

        [JsonPropertyName("cards3")]
        public List<HubLink> Cards3 { get; set; }

        [JsonPropertyName("sections")]
        public List<HubLink> Sections { get; set; }
    }

    public class HubEntryFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("slug")]
        public string Slug { get; set; }

        [JsonPropertyName("hubType")]
        public string HubType { get; set; }

        [JsonPropertyName("summary")]
        public string Summary { get; set; }

        [JsonPropertyName("metaDescription")]
        public string MetaDescription { get; set; }

        [JsonPropertyName("landingPage")]
        public HubLink LandingPage { get; set; }

        [JsonPropertyName("sectionType")]
        public string SectionType { get; set; }

        [JsonPropertyName("heading")]
        public string Heading { get; set; }

        [JsonPropertyName("introduction")]
        public string Introduction { get; set; }

        [JsonPropertyName("image")]
        public HubLink Image { get; set; }

        [JsonPropertyName("stepperLinks")]
        public List<HubLink> StepperLinks { get; set; }

        [JsonPropertyName("standardLinks")]
        public List<HubLink> StandardLinks { get; set; }

        [JsonPropertyName("ctaPanel")]
        public HubLink CtaPanel { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }

        [JsonPropertyName("buttonText")]
        public string ButtonText { get; set; }

        [JsonPropertyName("url")]
        public string Url { get; set; }
    }

    public class HubAssetFields
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("file")]
        public HubFile File { get; set; }
    }

    public class HubFile
    {
        [JsonPropertyName("url")]
        public string Url { get; set; }

        [JsonPropertyName("fileName")]
        public string FileName { get; set; }

        [JsonPropertyName("contentType")]
        public string ContentType { get; set; }

        [JsonPropertyName("details")]
        public HubFileDetails Details { get; set; }
    }

    public class HubFileDetails
    {
        [JsonPropertyName("size")]
        public long Size { get; set; }
    }
}
