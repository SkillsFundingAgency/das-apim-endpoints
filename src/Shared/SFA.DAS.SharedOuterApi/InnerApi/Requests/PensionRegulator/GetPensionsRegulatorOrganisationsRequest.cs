using System;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.PensionRegulator;

public class GetPensionsRegulatorOrganisationsRequest : IGetApiRequest
{
    public string Aorn { get; }
    public string PayeRef { get; }

    public GetPensionsRegulatorOrganisationsRequest(string aorn, string payeRef)
    {
        Aorn = aorn;
        PayeRef = Uri.EscapeDataString(payeRef);
    }

    public string GetUrl => $"/api/PensionsRegulator/organisations/{Aorn}?payeRef={PayeRef}";
}