using SFA.DAS.Aodp.InnerApi;

namespace SFA.DAS.Aodp.UnitTests.InnerApi;

[TestFixture]
public class MultipartFormDataMapperTests
{
    [Test]
    public void Map_WhenDataContainsSupportedShapes_ReturnsFlattenedFormData()
    {
        // Arrange
        var data = new
        {
            Name = "qualification",
            Count = 13_000,
            Enabled = true,
            Nested = new { Code = "QAN001" },
            Values = new object[] { "first", 2, false },
            Items = new[]
            {
                new { Qan = "10000001" },
                new { Qan = "10000002" }
            },
            Groups = new[]
            {
                new[] { "A", "B" },
                new[] { "C" }
            },
            EmptyText = string.Empty,
            Missing = (string?)null
        };
        KeyValuePair<string, string>[] expected =
        [
            new("Name", "qualification"),
            new("Count", "13000"),
            new("Enabled", "true"),
            new("Nested.Code", "QAN001"),
            new("Values", "first"),
            new("Values", "2"),
            new("Values", "false"),
            new("Items[0].Qan", "10000001"),
            new("Items[1].Qan", "10000002"),
            new("Groups[0]", "A"),
            new("Groups[0]", "B"),
            new("Groups[1]", "C"),
            new("EmptyText", string.Empty)
        ];

        // Act
        var result = MultipartFormDataMapper.Map(data).ToArray();

        // Assert
        Assert.That(result, Is.EqualTo(expected));
    }

    [Test]
    public void Map_WhenDataContainsOnlyNullAndEmptyCollections_ReturnsNoFormData()
    {
        // Arrange
        var data = new
        {
            Missing = (string?)null,
            EmptyItems = Array.Empty<object>(),
            EmptyObject = new { }
        };

        // Act
        var result = MultipartFormDataMapper.Map(data);

        // Assert
        Assert.That(result, Is.Empty);
    }
}
