using Med.Common.DataTransferObjects;
using Microsoft.AspNetCore.Mvc;

namespace Med.Common.Interfaces;

public interface IInspectionService
{
    public Task<InspectionModel> GetInfoAboutInspection(Guid id);
    public Task EditConcreteInspection(InspectionEditModel inspectionEditModel, Guid id, Guid doctorId);
    public Task<List<InspectionPreviewModel>> GetInspectionChain(Guid id);

}