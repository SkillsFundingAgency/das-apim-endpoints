using SFA.DAS.SharedOuterApi.Interfaces;
namespace SFA.DAS.AODP.Domain.FormBuilder.Requests.Forms;

public class CreateFormVersionApiRequest : IPostApiRequest
{
    public string PostUrl => "/api/forms";

    public object Data { get; set; }

    public class FormVersion
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int Order { get; set; }
    }
}