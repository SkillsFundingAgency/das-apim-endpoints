namespace SFA.DAS.Approvals.Application.SelectMultiple.Commands;

public class SelectMultipleAddDraftApprenticeshipsResult
{
    public SelectMultipleAddDraftApprenticeshipsResult() { }

    public string CohortReference { get; set; }
    public int NumberOfApprenticeships { get; set; }
    public string EmployerName { get; set; }

    public static implicit operator SelectMultipleAddDraftApprenticeshipsResult(InnerApi.Responses.BulkUploadAddDraftApprenticeshipsResponse response)
    {
        return new SelectMultipleAddDraftApprenticeshipsResult
        {
            CohortReference = response.CohortReference,
            NumberOfApprenticeships = response.NumberOfApprenticeships,
            EmployerName = response.EmployerName
        };
    }
}
