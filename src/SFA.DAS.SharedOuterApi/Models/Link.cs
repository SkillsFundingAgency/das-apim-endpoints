namespace SFA.DAS.SharedOuterApi.Models
{
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public string Type { get; set; } = "GET";
    }
}