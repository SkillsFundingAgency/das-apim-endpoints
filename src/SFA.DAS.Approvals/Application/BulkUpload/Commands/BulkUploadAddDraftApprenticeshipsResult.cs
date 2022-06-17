namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsResult
    {
        public BulkUploadAddDraftApprenticeshipsResult() { }

        public string CohortReference { get; set; }
        public int NumberOfApprenticeships { get; set; }
        public string EmployerName { get; set; }

        public static implicit operator BulkUploadAddDraftApprenticeshipsResult(InnerApi.Responses.BulkUploadAddDraftApprenticeshipsResponse response)
        {
            return new BulkUploadAddDraftApprenticeshipsResult
            {
                CohortReference = response.CohortReference,
                NumberOfApprenticeships = response.NumberOfApprenticeships,
                EmployerName = response.EmployerName
            };
        }
    }
}
