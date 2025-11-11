using Microsoft.AspNetCore.Http;
using Moq;
using SFA.DAS.Aodp.Application.Commands.Import;
using SFA.DAS.Aodp.Wrapper;
using SFA.DAS.SharedOuterApi.Interfaces;
using SFA.DAS.SharedOuterApi.Models;
using System.Net;
using System.Reflection;
using System.Text;

namespace SFA.DAS.Aodp.UnitTests.Application.Commands.Import;

[TestFixture]
public class ImportDefundingListCommandHandlerTests
{
    private Mock<IMultipartFormDataSenderWrapper> _multipartWrapperMock = null!;
    private ImportDefundingListCommandHandler _handler = null!;
    private const string FieldName = "_multipartFormDataSenderWrapper";

    [SetUp]
    public void SetUp()
    {
        _multipartWrapperMock = new Mock<IMultipartFormDataSenderWrapper>();
        _handler = new ImportDefundingListCommandHandler(_multipartWrapperMock.Object);
    }

    [Test]
    public void Constructor_WithValidDependency_AssignsDependency()
    {
        // Act
        var handler = new ImportDefundingListCommandHandler(_multipartWrapperMock.Object);

        // Assert
        Assert.Multiple(() =>
        {
            var field = handler.GetType().GetField(FieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, $"Expected private field '{FieldName}' to exist.");
            var actualValue = field.GetValue(handler);
            Assert.That(actualValue, Is.SameAs(_multipartWrapperMock.Object));
        });
    }

    [Test]
    public void Constructor_WithNull_DoesNotThrowAndLeavesFieldNull()
    {
        // Arrange / Act
        ImportDefundingListCommandHandler? handler = null;
        Assert.DoesNotThrow(() => handler = new ImportDefundingListCommandHandler(null!));

        // Assert
        Assert.Multiple(() =>
        {
            var field = handler?.GetType().GetField(FieldName, BindingFlags.Instance | BindingFlags.NonPublic);
            Assert.That(field, Is.Not.Null, $"Expected private field '{FieldName}' to exist.");
            var actualValue = field.GetValue(handler);
            Assert.That(actualValue, Is.Null);
        });
    }

    [Test]
    public async Task Handle_WhenWithValidFile_ShouldReturnsSuccessAndValue()
    {
        // Arrange
        var file = CreateFormFile("a,b,c\n1,2,3");
        var expectedResponseBody = new ImportDefundingListResponse
        {
            ImportedCount = 42,
            Message = "Imported"
        };
        var apiResponse = new ApiResponse<ImportDefundingListResponse>(expectedResponseBody, HttpStatusCode.OK, string.Empty, new Dictionary<string, IEnumerable<string>>());
        _multipartWrapperMock
            .Setup(m => m.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(It.IsAny<IPostApiRequest<IFormFile>>(), It.IsAny<bool>()))
            .ReturnsAsync(apiResponse);

        var command = new ImportDefundingListCommand { File = file };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.True);
            Assert.That(result.ErrorMessage, Is.Null);
            Assert.That(result.Value, Is.Not.Null);
            Assert.That(result.Value.ImportedCount, Is.EqualTo(expectedResponseBody.ImportedCount));
            Assert.That(result.Value.Message, Is.EqualTo(expectedResponseBody.Message));
            _multipartWrapperMock.Verify(m => m.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>
                    (It.IsAny<IPostApiRequest<IFormFile>>(), It.IsAny<bool>()), Times.Once);
        });

    }

    [Test]
    public async Task Handle_WhenWrapperThrowsException_ShouldReturnsFailureAndErrorMessage()
    {
        // Arrange
        var file = CreateFormFile("a,b");
        var expectedException = new InvalidOperationException("Wrapper failure");
        _multipartWrapperMock
            .Setup(m => m.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(It.IsAny<IPostApiRequest<IFormFile>>(), It.IsAny<bool>()))
            .ThrowsAsync(expectedException);

        var command = new ImportDefundingListCommand { File = file };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Multiple(() =>
        {
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Success, Is.False);
            Assert.That(result.ErrorMessage, Is.Not.Null);
            Assert.That(result.ErrorMessage, Is.EqualTo(expectedException.Message));
            _multipartWrapperMock.Verify(m => m.PostWithMultipartFormData<IFormFile, ImportDefundingListResponse>(It.IsAny<IPostApiRequest<IFormFile>>(), It.IsAny<bool>()), Times.Once);
        });
    }

    private static IFormFile CreateFormFile(string content, string fileName = "file.csv")
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var stream = new MemoryStream(bytes);
        var formFile = new FormFile(stream, 0, stream.Length, "file", fileName)
        {
            Headers = new HeaderDictionary(),
            ContentType = "text/csv"
        };
        return formFile;
    }
}
