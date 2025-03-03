using System.Text.Json.Serialization;
using Newtonsoft.Json;

namespace SFA.DAS.SharedOuterApi.Models
{
    public record LocationItem(string Name, double[] GeoPoint, string Country);
}