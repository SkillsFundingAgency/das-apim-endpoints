using System;
using System.Text.Json.Serialization;
using Microsoft.Azure.Amqp.Framing;
using SFA.DAS.FindAnApprenticeship.InnerApi.CandidateApi.Responses;

namespace SFA.DAS.FindAnApprenticeship.Api.Models.Applications.Qualifications;

public class QualificationTypeApiResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("order")]
    public long Order { get; set; }

    public static implicit operator QualificationTypeApiResponse(QualificationReference source)
    {
        return new QualificationTypeApiResponse
        {
            Id = source.Id,
            Name = source.Name,
            Order = source.Order
        };
    }
}