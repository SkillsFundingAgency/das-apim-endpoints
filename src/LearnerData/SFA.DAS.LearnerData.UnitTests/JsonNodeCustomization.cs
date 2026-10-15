using System.Text.Json.Nodes;
using AutoFixture;

namespace SFA.DAS.LearnerData.UnitTests;

/// <summary>
/// AutoFixture cannot build the abstract <see cref="JsonNode"/>, which is the type of the old/new values in the
/// Learning response's change details. Apply this to any fixture that creates one of those responses.
/// </summary>
public class JsonNodeCustomization : ICustomization
{
    public void Customize(IFixture fixture) =>
        fixture.Register<JsonNode>(() => JsonValue.Create(fixture.Create<string>()));
}
