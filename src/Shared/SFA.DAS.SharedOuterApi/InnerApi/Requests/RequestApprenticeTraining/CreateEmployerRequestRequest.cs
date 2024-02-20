using SFA.DAS.SharedOuterApi.Interfaces;

namespace SFA.DAS.SharedOuterApi.InnerApi.Requests.RequestApprenticeTraining
{
    public class CreateEmployerRequestRequest : IPostApiRequest<CreateEmployerRequestData>
    {
        public string PostUrl => "api/employerrequest";

        public CreateEmployerRequestData Data { get; set; }

        public CreateEmployerRequestRequest(CreateEmployerRequestData data)
        {
            Data = data;
        }
    }

    public class CreateEmployerRequestData
    {
        public RequestType RequestType { get; set; }
    }

    public enum RequestType
    {
        Shortlist = 1
    }
}
