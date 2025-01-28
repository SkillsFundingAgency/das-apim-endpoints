using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Questions;

public class UpdateQuestionApiRequest(Guid questionId, Guid pageId, Guid formVersionId, Guid sectionId) : IPutApiRequest
{
    public Guid PageId { get; set; } = pageId;
    public Guid FormVersionId { get; set; } = formVersionId;
    public Guid SectionId { get; set; } = sectionId;
    public Guid QuestionId { get; set; } = questionId;

    public string PutUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}/Questions/{QuestionId}";

    public object Data { get; set; }

}