using SFA.DAS.SharedOuterApi.Interfaces;


namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class UpdateFormVersionApiRequest : IPutApiRequest
{
    public Guid FormVersionId { get; set; }
    public object Data { get; set; }

    public string PutUrl => $"/api/forms/{FormVersionId}";


    public class FormVersion
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}