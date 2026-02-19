using System;
using System.ClientModel;
using System.ClientModel.Primitives;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading;
using OpenAI.Chat;
using SFA.DAS.RecruitJobs.Ai.Clients;

namespace SFA.DAS.RecruitJobs.UnitTests.Ai;

public class WhenGettingAzureAiResponse
{
    public class MockPipelineResponse : PipelineResponse
    {
        public override BinaryData BufferContent(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override ValueTask<BinaryData> BufferContentAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            throw new NotImplementedException();
        }

        public override int Status => (int)HttpStatusCode.TooManyRequests;
        public override string ReasonPhrase { get; }
        protected override PipelineResponseHeaders HeadersCore { get; }
        public override Stream? ContentStream { get; set; }
        public override BinaryData Content { get; }
    }
    
    [Test]
    public void Then_Exceptions_Are_Mapped_Correctly()
    {
        // arrange
        var exception = new ClientResultException("message contents", new MockPipelineResponse());

        // act
        var actual = AzureAiResponse<string>.From(exception);

        // assert
        actual.StatusCode.Should().Be((HttpStatusCode)exception.Status);
        actual.Refusal.Should().Be(exception.Message);
    }
    
    [Test]
    public void Then_Empty_Results_Are_Mapped_Correctly()
    {
        // arrange
        var pipelineResponse = new MockPipelineResponse();
        //var chatCompletion = OpenAIChatModelFactory.ChatCompletion(role: ChatMessageRole.User, refusal: "some rejection text");
        var clientResult = ClientResult.FromOptionalValue<ChatCompletion>(null, pipelineResponse);
        
        // act
        var actual = AzureAiResponse<string>.From(clientResult);

        // assert
        actual.StatusCode.Should().Be((HttpStatusCode)pipelineResponse.Status);
        actual.Refusal.Should().BeNull();
        actual.RawResult.Should().BeNull();
        actual.Result.Should().BeNull();
    }
    
    [Test]
    public void Then_Results_Are_Mapped_Correctly()
    {
        // arrange
        var pipelineResponse = new MockPipelineResponse();
        var contentPart = ChatMessageContentPart.CreateTextPart("{ \"title\": \"title text\" }");
        var chatCompletion = OpenAIChatModelFactory.ChatCompletion(role: ChatMessageRole.User, refusal: "rejection text", content: [contentPart]);
        var clientResult = ClientResult.FromValue(chatCompletion, pipelineResponse);
        
        // act
        var actual = AzureAiResponse<Dictionary<string, string>>.From(clientResult);

        // assert
        actual.StatusCode.Should().Be((HttpStatusCode)pipelineResponse.Status);
        actual.Refusal.Should().Be(chatCompletion.Refusal);
        actual.RawResult.Should().Be(contentPart.Text);
        actual.Result.Should().NotBeNull();
        actual.Result.Should().ContainEquivalentOf(new KeyValuePair<string, string>("title", "title text"));
    }
    
    [Test]
    public void Then_Invalid_Result_Text_Does_Not_Cause_An_Error()
    {
        // arrange
        var pipelineResponse = new MockPipelineResponse();
        var contentPart = ChatMessageContentPart.CreateTextPart("this text should be a json object of the expected result type");
        var chatCompletion = OpenAIChatModelFactory.ChatCompletion(role: ChatMessageRole.User, refusal: "rejection text", content: [contentPart]);
        var clientResult = ClientResult.FromValue(chatCompletion, pipelineResponse);
        
        // act
        var actual = AzureAiResponse<Dictionary<string, string>>.From(clientResult);

        // assert
        actual.StatusCode.Should().Be((HttpStatusCode)pipelineResponse.Status);
        actual.Refusal.Should().Be(chatCompletion.Refusal);
        actual.RawResult.Should().Be(contentPart.Text);
        actual.Result.Should().BeNull();
    }
}