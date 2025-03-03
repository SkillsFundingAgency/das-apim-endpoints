using System.Linq;

namespace SFA.DAS.SharedOuterApi.InnerApi.Responses.PensionsRegulator;
public class PensionRegulatorOrganisation
{
    public string Name { get; set; }
    public string Status { get; set; }
    public long UniqueIdentity { get; set; }
    public Address Address { get; set; }
}

public class Address
{
    public string Line1 { get; set; }
    public string Line2 { get; set; }
    public string Line3 { get; set; }
    public string Line4 { get; set; }
    public string Line5 { get; set; }
    public string Postcode { get; set; }
    public override string ToString()
    {
        string[] values = [Line1, Line2, Line3, Line4, Line5, Postcode];
        return string.Join(", ", values.Where(v => !string.IsNullOrWhiteSpace(v)));
    }
}
