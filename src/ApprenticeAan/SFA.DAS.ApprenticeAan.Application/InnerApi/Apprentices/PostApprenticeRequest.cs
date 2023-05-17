using System.Diagnostics.CodeAnalysis;
using SFA.DAS.ApprenticeAan.Application.Apprentices.Commands.CreateApprenticeMember;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.ApprenticeAan.Application.InnerApi.Apprentices;

[ExcludeFromCodeCoverage]
public class PostApprenticeRequest : IPostApiRequest
{
    public string PostUrl => "apprentices";

    public object Data { get; set; }

    public PostApprenticeRequest(CreateApprenticeMemberCommand data)
    {
        Data = data;
    }
}
