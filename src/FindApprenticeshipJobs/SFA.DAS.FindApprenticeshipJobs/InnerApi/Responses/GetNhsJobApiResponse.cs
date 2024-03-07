using System.Xml.Serialization;

namespace SFA.DAS.FindApprenticeshipJobs.InnerApi.Responses;

[XmlRoot(ElementName = "nhsJobs")]  
public class GetNhsJobApiResponse
{
    [XmlElement(ElementName = "vacancyDetails")]
    public List<GetNhsJobApiDetailResponse> Vacancies { get; set; } = [];  
    [XmlElement(ElementName = "totalPages")]
    public int TotalPages { get; set; }
    [XmlElement(ElementName = "totalResults")]
    public int TotalResults { get; set; }
}

public class GetNhsJobApiDetailResponse
{
    [XmlElement(ElementName = "id")]
    public string Id { get; set; }
    [XmlElement(ElementName = "reference")]
    public string Reference { get; set; }
    [XmlElement(ElementName = "title")]
    public string Title { get; set; }
    [XmlElement(ElementName = "description")]
    public string Description { get; set; }
    [XmlElement(ElementName = "employer")]
    public string Employer { get; set; }
    [XmlElement(ElementName = "type")]
    public string Type { get; set; }
    [XmlElement(ElementName = "salary")]
    public string Salary { get; set; }
    [XmlElement(ElementName = "closeDate")]
    public string CloseDate { get; set; }
    [XmlElement(ElementName = "postDate")]
    public string PostDate { get; set; }
    [XmlElement(ElementName = "url")]
    public string Url { get; set; }

    [XmlElement(ElementName = "locations")]
    public List<GetNhsJobLocationApiResponse> Locations { get; set; } = [];
}

public class GetNhsJobLocationApiResponse
{
    [XmlElement(ElementName = "location")] 
    public string Location { get; set; }
}
