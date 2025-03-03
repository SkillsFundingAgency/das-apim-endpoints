using SFA.DAS.SharedOuterApi.Configuration;
using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;

public class GetQuestionByIdApiRequest : IGetApiRequest
{
    public Guid QuestionId { get; set; }
    public Guid PageId { get; set; }
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }

    public string GetUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}/Questions/{QuestionId}";
}