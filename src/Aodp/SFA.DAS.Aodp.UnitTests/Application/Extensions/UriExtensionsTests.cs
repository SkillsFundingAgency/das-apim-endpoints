using System.Collections.Specialized;
using SFA.DAS.Aodp.Application.Extensions;

namespace SFA.DAS.Aodp.UnitTests.Application.Extensions
{
    [TestFixture]
    public class UriExtensionTests
    {

        [Test]
        public void AttachParameters_ToUri_ReturnsUriWithQueryString()
        {
            // Arrange
            var uri = new Uri("https://test.com/path");

            var parameters = new NameValueCollection
            {
                { "param1", "value1" },
                { "param2", "value2" }
            };

            // Act
            var result = uri.AttachParameters(parameters);

            // Assert
            Assert.That(result.ToString(), Is.EqualTo("https://test.com/path?param1=value1&param2=value2"));
        }

        [Test]
        public void AttachParameters_ToString_ReturnsStringWithQueryString()
        {
            // Arrange
            var uri = "https://test.com/path";

            var parameters = new NameValueCollection
            {
                { "param1", "value1" },
                { "param2", "value2" }
            };

            // Act
            var result = uri.AttachParameters(parameters);

            // Assert
            Assert.That(result, Is.EqualTo("https://test.com/path?param1=value1&param2=value2"));
        }

        [Test]
        public void AttachMultiValueParameters_ReturnsStringWithMultipleValues()
        {
            // Arrange
            var uri = "https://test.com/path";

            var parameters = new NameValueCollection
            {
                { "status", "Approved" },
                { "status", "Pending" },
                { "reviewer", "John" }
            };

            // Act
            var result = uri.AttachMultiValueParameters(parameters);

            // Assert
            Assert.That(result, Is.EqualTo("https://test.com/path?status=Approved&status=Pending&reviewer=John"));
        }

        [Test]
        public void AttachMultiValueParameters_EncodesSpecialCharacters()
        {
            // Arrange
            var uri = "https://test.com/path";

            var parameters = new NameValueCollection
            {
                { "full name", "John Smith" },
                { "email", "john+test@test.com" }
            };

            // Act
            var result = uri.AttachMultiValueParameters(parameters);

            // Assert
            Assert.That(result, Is.EqualTo("https://test.com/path?full%20name=John%20Smith&email=john%2Btest%40test.com"));
        }

        [Test]
        public void AttachMultiValueParameters_ReturnsOriginalUri_WhenParametersAreEmpty()
        {
            // Arrange
            var uri = "https://test.com/path";

            var parameters = new NameValueCollection();

            // Act
            var result = uri.AttachMultiValueParameters(parameters);

            // Assert
            Assert.That(result, Is.EqualTo("https://test.com/path"));
        }

        [Test]
        public void AttachParameters_ReturnsOriginalUri_WhenParametersAreEmpty()
        {
            // Arrange
            var uri = "https://test.com/path";

            var parameters = new NameValueCollection();

            // Act
            var result = uri.AttachParameters(parameters);

            // Assert
            Assert.That(result, Is.EqualTo("https://test.com/path"));
        }
    }
}