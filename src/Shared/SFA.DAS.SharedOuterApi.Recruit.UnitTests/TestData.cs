using System.Reflection;
using System.Resources;
using System.Text;

namespace SFA.DAS.SharedOuterApi.Recruit.UnitTests;

public static class TestData
{
    public static readonly string BankHolidaysJson = ReadResourceAsString("BankHolidays.json").Result;

    private static async Task<string> ReadResourceAsString(string resourceName)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var name = assembly.GetManifestResourceNames().FirstOrDefault(x => x.EndsWith(resourceName));
        if (name == null)
        {
            throw new MissingManifestResourceException($"Could not find requested resource {resourceName}");
        }
        
        var stream = assembly.GetManifestResourceStream(name)!;
        using var reader = new StreamReader(stream, Encoding.UTF8);
        return await reader.ReadToEndAsync();
    }
}