using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.Aodp.Domain.FormBuilder.Requests.Questions;

public class CreateQuestionApiRequest : IPostApiRequest
{
    public Guid FormVersionId { get; set; }
    public Guid SectionId { get; set; }
    public Guid PageId { get; set; }

    public string PostUrl => $"/api/forms/{FormVersionId}/sections/{SectionId}/pages/{PageId}/questions";

    public object Data { get; set; }
}
