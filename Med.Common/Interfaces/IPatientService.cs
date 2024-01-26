using Med.Common.DataTransferObjects;
using Med.Common.Enums;
using Med.Common.Other;

namespace Med.Common.Interfaces;

public interface IPatientService
{
    public Task<Guid> CreateNewPatient(PatientCreateModel patientCreateModel);
    public Task<PatientModel> GetPatientCard(Guid id);
    public Task<PagedList<PatientModel>> GetPatientList(string? name, List<Conclusion>? conclusions, Sorting? sorting, bool? scheduledVisits, bool? onlyMine, int page, int size, Guid userId);
    public Task<Guid> CreateInspectionForPatient(InspectionCreateModel inspectionCreateModel, Guid id, Guid doctorId);
    public Task<List<InspectionShortModel>> SearchForPatientInspections(string? request, Guid patientId);
    public Task<PagedList<InspectionPreviewModel>> GetListOfPatientInspections(Guid patientId, bool? grouped, List<string>? icdRoots, int page, int size);
}