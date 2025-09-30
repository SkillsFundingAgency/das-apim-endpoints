using System;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp;
public class BaseEntity
{
    public string CreatedBy { get; set; }
    public DateTime CreatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string Status { get; set; }
}