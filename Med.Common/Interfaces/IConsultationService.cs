using Med.Common.DataTransferObjects;
using Med.Common.Other;

namespace Med.Common.Interfaces;

public interface IConsultationService
{
    public Task<PagedList<InspectionPreviewModel>> GetConsultationList(bool? grouped, List<string> icdRoots, int page, int size);
    public Task<ConsultationModel> GetConsultation(Guid consultationId);
    public Task<Guid> AddCommentToConsultation(CommentCreateModel commentCreateModel, Guid consultationId, Guid userId);
    public Task EditConsultationComment(InspectionCommentCreateModel inspectionCommentCreateModel, Guid commentId, Guid userId);
}