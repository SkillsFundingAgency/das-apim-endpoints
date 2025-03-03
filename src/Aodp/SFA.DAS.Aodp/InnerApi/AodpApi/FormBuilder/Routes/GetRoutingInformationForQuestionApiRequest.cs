using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Routes;

public class GetRoutingInformationForQuestionApiRequest : IGetApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public Guid PageId { get; set; }
    public Guid QuestionId { get; set; }


    public string GetUrl => $"/api/routes/forms/{FormVersionId}/sections/{SectionId}/pages/{PageId}/questions/{QuestionId}";
}