namespace SFA.DAS.Approvals.Application.BulkUpload.Commands
{
    public class BulkUploadAddDraftApprenticeshipsResponse
    {
        public BulkUploadAddDraftApprenticeshipsResponse() { }

        public string CohortReference { get; set; }
        public int NumberOfApprenticeships { get; set; }
        public string EmployerName { get; set; }
    }
}
