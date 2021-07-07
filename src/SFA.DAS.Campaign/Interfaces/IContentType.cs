using Contentful.Core.Models;

namespace SFA.DAS.Campaign.Interfaces
{
    public interface IContentType
    {
        SystemProperties Sys { get; set; }
        string Slug { get; set; }
        string Title { get; set; }
    }
}