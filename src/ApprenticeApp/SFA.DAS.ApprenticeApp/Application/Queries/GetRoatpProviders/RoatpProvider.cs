using System.Diagnostics.CodeAnalysis;

namespace SFA.DAS.ApprenticeApp.Application.Queries.GetRoatpProviders;

[ExcludeFromCodeCoverage]
public class RoatpProvider
{
    public string Name { get; set; }
    public int Ukprn { get; set; }    
}