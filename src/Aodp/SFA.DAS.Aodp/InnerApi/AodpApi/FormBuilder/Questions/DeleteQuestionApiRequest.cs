using SFA.DAS.Aodp.Configuration;
using SFA.DAS.Aodp.Services;
using SFA.DAS.SharedOuterApi.Types.Configuration;

using SFA.DAS.SharedOuterApi.Types.Interfaces;
using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.Aodp.InnerApi.AodpApi.FormBuilder.Questions;
public class DeleteQuestionApiRequest(Guid questionId, Guid pageId, Guid formVersionId, Guid sectionId) : IDeleteApiRequest
{
    public Guid PageId { get; set; } = pageId;
    public Guid FormVersionId { get; set; } = formVersionId;
    public Guid SectionId { get; set; } = sectionId;
    public Guid QuestionId { get; set; } = questionId;
    public string DeleteUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/Pages/{PageId}/Questions/{QuestionId}";
    public object Data { get; set; }
}