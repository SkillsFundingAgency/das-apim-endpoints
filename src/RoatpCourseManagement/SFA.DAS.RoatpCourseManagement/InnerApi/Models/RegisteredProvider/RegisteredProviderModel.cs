using System;
using System.Collections.Generic;
using SFA.DAS.SharedOuterApi.InnerApi.Responses.Roatp.Common;

namespace SFA.DAS.RoatpCourseManagement.InnerApi.Models.RegisteredProvider;

public class RegisteredProviderModel
{
    public int Ukprn { get; set; }
    public OrganisationStatus Status { get; set; }
    public DateTime StatusDate { get; set; }
    public int OrganisationTypeId { get; set; }
    public ProviderType ProviderType { get; set; }
    public string LegalName { get; set; }
}

public class RegisteredProviderResponse
{
    public List<RegisteredProviderModel> Organisations { get; set; } = [];
}
